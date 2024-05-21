namespace RabbitMq.Interfaces;

public interface IMessageQueueProducer<in T>
    where T : IQueueMessage
{
    void Send(T message);
}