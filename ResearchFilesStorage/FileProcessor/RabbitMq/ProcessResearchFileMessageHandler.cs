using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMq.Extensions;
using RabbitMq.Messages;

namespace FileProcessor.RabbitMq;

public class ProcessResearchFileMessageHandler(
    IProcessFileMessageCompletedQueueProducer messageQueueProducer,
    ILogger<ProcessResearchFileMessageHandler> logger) : IRequestHandler<HandleMessage<ProcessResearchFileMessage>>
{
    public async Task Handle(HandleMessage<ProcessResearchFileMessage> request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Saving ResearchFile with Id: {Id} on local storage", request.Message!.Id);
        // simulate saving file to local storage
        await Task.Delay(500, cancellationToken);

        var message = new ProcessResearchFileCompletedMessage()
        {
            Id = request.Message!.Id,
            SavedLocally = true
        };

        messageQueueProducer.Send(message);
    }
}
