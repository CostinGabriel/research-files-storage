using RabbitMq.Interfaces;
using RabbitMq.Producer;
using RabbitMQ.Client;

namespace FileProcessor.RabbitMq;

public interface IProcessFileMessageCompletedQueueProducer : IMessageQueueProducer<IQueueMessage>
{

}

public class ProcessFileMessageCompletedQueueProducer : RabbitMqProducer<IQueueMessage>, IProcessFileMessageCompletedQueueProducer
{
    public ProcessFileMessageCompletedQueueProducer(IConnection connection, string queueName)
        : base(connection, queueName)
    {

    }
}