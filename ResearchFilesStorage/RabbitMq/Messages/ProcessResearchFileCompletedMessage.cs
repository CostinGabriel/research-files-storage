namespace RabbitMq.Messages;

public class ProcessResearchFileCompletedMessage : BaseQueueMessage
{
    public Guid Id { get; set; }
    public bool SavedLocally { get; set; }
}
