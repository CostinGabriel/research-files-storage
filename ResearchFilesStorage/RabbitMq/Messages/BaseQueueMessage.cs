using RabbitMq.Interfaces;

namespace RabbitMq.Messages;

[Serializable]
public class BaseQueueMessage : IQueueMessage
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    protected BaseQueueMessage() { }
}