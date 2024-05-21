using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMq.Configuration;
using RabbitMq.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.RegularExpressions;

namespace RabbitMq.Consumer;

public abstract class RabbitMqConsumerBase : BackgroundService
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };

    private readonly ILogger<RabbitMqConsumerBase> _logger;

    private readonly Dictionary<string, object> queueArgs = new()
        {
            {"x-max-priority", 10},
            {"ha-mode", "all"}
        };

    protected readonly IModel Channel;
    private readonly EventingBasicConsumer consumer;
    protected readonly QueueConfiguration queueConfig;

    protected abstract Task HandleMessage(BaseQueueMessage message, CancellationToken cancellationToken);

    protected RabbitMqConsumerBase(IConnection connection, QueueConfiguration queueConfiguration, ILogger<RabbitMqConsumerBase> logger)
    {
        if (string.IsNullOrEmpty(queueConfiguration?.QueueName))
        {
            throw new ArgumentNullException(nameof(queueConfiguration));
        }

        Channel = connection.CreateModel();
        consumer = new EventingBasicConsumer(Channel);

        queueConfig = queueConfiguration;
        _logger = logger;
    }


    public override Task StartAsync(CancellationToken cancellationToken)
    {
        Channel.BasicQos(0, queueConfig.QosPrefetchCount, false);

        Channel.QueueDeclare(queueConfig.QueueName,
            true,
            false,
            false,
            queueArgs);

        consumer.Received += ConsumerOnReceived(cancellationToken);
        Channel.BasicConsume(queueConfig.QueueName, false, consumer);
        return base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (Channel == null)
            return;

        if (Channel.IsOpen)
        {
            Channel.BasicCancel(consumer.ConsumerTags.First());
            Channel.Close();
        }

        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }

    private EventHandler<BasicDeliverEventArgs> ConsumerOnReceived(CancellationToken cancellationToken)
    {
        return async delegate (object? _, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                var message = FromBinary(eventArgs.Body.Span);

                await HandleMessage(message, cancellationToken);
                Channel.BasicAck(eventArgs.DeliveryTag, false);
                _logger.Log(LogLevel.Information, "Consumed message");
            }
            catch (Exception ex)
            {
                try
                {
                    var message = ToString(eventArgs.Body.Span);
                    _logger.Log(LogLevel.Error, ex,
                        $"An error occured while processing the message: {message}");
                }
                catch (Exception)
                {
                    _logger.Log(LogLevel.Error, ex,
                        "An error occured while processing the message. Could not extract message string");
                }
                finally
                {
                    // Depending on the context you would have to ack the message and leave the flow in an error like state 
                    // Or nack the message which will push it back in the queue and will be reprocessed
                    // Or will be pushed in the dead-letter queue if it exists
                    Channel.BasicAck(eventArgs.DeliveryTag, false);
                }
            }
        };
    }

    private static BaseQueueMessage FromBinary(ReadOnlySpan<byte> bytes)
    {
        var content = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<BaseQueueMessage>(content, JsonSettings)!;
    }

    private static string ToString(ReadOnlySpan<byte> bytes)
    {
        var content = Encoding.UTF8.GetString(bytes);
        return Regex.Unescape(content);
    }
}