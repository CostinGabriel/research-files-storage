using RabbitMq.Interfaces;
using RabbitMq.Producer;
using RabbitMQ.Client;

namespace ResearchFilesStorage.RabbitMq;

public interface IProcessFileMessageQueueProducer : IMessageQueueProducer<IQueueMessage>
{
}

public class ProcessFileMessageQueueProducer : RabbitMqProducer<IQueueMessage>, IProcessFileMessageQueueProducer
{
    public ProcessFileMessageQueueProducer(IConnection connection, string queueName)
        : base(connection, queueName)
    {

    }
}