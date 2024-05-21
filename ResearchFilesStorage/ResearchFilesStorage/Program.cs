using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using ResearchFilesStorage.Domain.Repository;
using ResearchFilesStorage.Infrastructure;
using System.Text.Json.Serialization;
using ResearchFilesStorage.Application;
using ResearchFilesStorage.Domain.Builders;
using ResearchFilesStorage.Application.Validation;
using MediatR;
using Microsoft.Extensions.Options;
using RabbitMq.Configuration;
using RabbitMq.Extensions;
using RabbitMq.Consumer;
using RabbitMQ.Client;
using ResearchFilesStorage.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                     .AddEnvironmentVariables()
                     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false, reloadOnChange: true);

//Add mongoDb
builder.Services.Configure<ResearchFileDatabaseSettings>(builder.Configuration.GetSection("ResearchFileDatabase"));
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
builder.Services.AddScoped<IResearchFileRepository, ResearchFileRepository>();

builder.Services.AddTransient<IFileBuilder, FileBuilder>();
builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();

//Add mediator 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});


//Add rabbitmq
builder.Services.Configure<OutputQueueNamesConfiguration>(builder.Configuration.GetSection(StorageConstants.OUTPUT_QUEUE_NAMES_CONFIGURATION_SECTION_NAME));
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection(StorageConstants.RABBIT_MQ_CONFIGURATION_SECTION_NAME));
builder.Services.Configure<InputQueueConfiguration>(builder.Configuration.GetSection(StorageConstants.INPUT_QUEUE_CONFIGURATION_SECTION_NAME));
builder.Services.AddSingleton(p =>
{
    var rabbitMqSettings = p.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
    return RabbitMqHelper.GetConnection(rabbitMqSettings);
});

//Add producer
builder.Services.AddSingleton<IProcessFileMessageQueueProducer, ProcessFileMessageQueueProducer>(p =>
{
    var queueName = builder.Configuration.GetSection(StorageConstants.OUTPUT_QUEUE_NAMES_CONFIGURATION_SECTION_NAME)[nameof(OutputQueueNamesConfiguration.ResearchFileProcessQueueName)];
    return new ProcessFileMessageQueueProducer(p.GetRequiredService<IConnection>(), queueName!);
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
