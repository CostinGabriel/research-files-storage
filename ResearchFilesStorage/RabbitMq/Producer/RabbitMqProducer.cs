using Newtonsoft.Json;
using RabbitMq.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMq.Producer;

public class RabbitMqProducer<T> : IDisposable, IMessageQueueProducer<T> where T : IQueueMessage
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly Dictionary<string, object> queueArgs = new()
        {
            {"x-max-priority", 10},
            {"ha-mode", "all"}
        };

    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    public RabbitMqProducer(IConnection connection, string queueName)
    {
        _connection = connection;
        _channel = connection.CreateModel();
        _queueName = queueName;

        _channel.QueueDeclare(queueName, true, false, false, queueArgs);
    }

    public void Send(T message)
    {
        var binaryMessage = ToBinary(message);
        var properties = _channel.CreateBasicProperties();
        properties.ContentType = "application/json";
        properties.ContentEncoding = "gzip";
        _channel.BasicPublish(string.Empty, _queueName, properties, binaryMessage);
    }

    private static byte[] ToBinary(IQueueMessage message)
    {
        var jsonMessage = JsonConvert.SerializeObject(message, JsonSettings);
        return Encoding.UTF8.GetBytes(jsonMessage);
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
        GC.SuppressFinalize(this);
    }
}