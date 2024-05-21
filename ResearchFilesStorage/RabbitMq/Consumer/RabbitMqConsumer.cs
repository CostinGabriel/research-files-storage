using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMq.Configuration;
using RabbitMq.Extensions;
using RabbitMq.Messages;
using RabbitMQ.Client;

namespace RabbitMq.Consumer;

public class RabbitMqConsumer : RabbitMqConsumerBase
{
    private readonly IMediator _mediator;

    public RabbitMqConsumer(
        IConnection connection,
        QueueConfiguration queueConfiguration,
        IMediator mediator,
        ILogger<RabbitMqConsumerBase> logger)
        : base(connection, queueConfiguration, logger)
    {
        _mediator = mediator;
    }

    protected override async Task HandleMessage(BaseQueueMessage message, CancellationToken cancellationToken)
    {
        await _mediator.Send(message.CreateHandleMessageWrapper(), cancellationToken);
    }
}