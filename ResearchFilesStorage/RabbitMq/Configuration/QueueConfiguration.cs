namespace RabbitMq.Configuration;

public class InputQueueConfiguration
{
    public QueueConfiguration[]? InputQueues { get; set; }
}

public class QueueConfiguration
{
    public string? QueueName { get; set; }
    public ushort QosPrefetchCount { get; set; }
}
