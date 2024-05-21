using FileProcessor.RabbitMq;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMq.Configuration;
using RabbitMq.Consumer;
using RabbitMq.Extensions;
using RabbitMQ.Client;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                     .AddEnvironmentVariables()
                     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false, reloadOnChange: true);

//Add RabbitMq
builder.Services.Configure<OutputQueueNamesConfiguration>(builder.Configuration.GetSection(StorageConstants.OUTPUT_QUEUE_NAMES_CONFIGURATION_SECTION_NAME));
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection(StorageConstants.RABBIT_MQ_CONFIGURATION_SECTION_NAME));
builder.Services.Configure<InputQueueConfiguration>(builder.Configuration.GetSection(StorageConstants.INPUT_QUEUE_CONFIGURATION_SECTION_NAME));
builder.Services.AddSingleton(p =>
{
    var rabbitMqSettings = p.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
    return RabbitMqHelper.GetConnection(rabbitMqSettings);
});

//Add producer
builder.Services.AddSingleton<IProcessFileMessageCompletedQueueProducer, ProcessFileMessageCompletedQueueProducer>(p =>
{
    var queueName = builder.Configuration.GetSection(StorageConstants.OUTPUT_QUEUE_NAMES_CONFIGURATION_SECTION_NAME)[nameof(OutputQueueNamesConfiguration.ResearchFileProcessQueueName)];
    return new ProcessFileMessageCompletedQueueProducer(p.GetRequiredService<IConnection>(), queueName!);
});

//Add consumer
var inputQueueConfig = new InputQueueConfiguration();
builder.Configuration.GetSection(StorageConstants.INPUT_QUEUE_CONFIGURATION_SECTION_NAME).Bind(inputQueueConfig);
foreach (var queConfig in inputQueueConfig.InputQueues!)
{
    builder.Services.Add(ServiceDescriptor.Singleton<IHostedService>(p =>
    {
        var mediator = p.GetRequiredService<IMediator>();
        var connection = p.GetRequiredService<IConnection>();
        var logger = p.GetRequiredService<ILogger<RabbitMqConsumerBase>>();
        return new RabbitMqConsumer(connection, queConfig, mediator, logger);
    }));
}

//Add mediator 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ProcessResearchFileMessageHandler).Assembly);
});

using IHost host = builder.Build();

await host.RunAsync();