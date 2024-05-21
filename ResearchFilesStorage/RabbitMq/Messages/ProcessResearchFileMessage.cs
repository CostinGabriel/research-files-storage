namespace RabbitMq.Messages;

public class ProcessResearchFileMessage : BaseQueueMessage
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public string? Name { get; set; }
    public string? Extension { get; set; }
}
