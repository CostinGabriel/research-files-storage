using MediatR;
using RabbitMq.Extensions;
using RabbitMq.Messages;
using ResearchFilesStorage.Domain.Repository;

namespace ResearchFilesStorage.RabbitMq;

public class ProcessResearchFileCompletedMessageHandler(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ProcessResearchFileCompletedMessageHandler> logger) : IRequestHandler<HandleMessage<ProcessResearchFileCompletedMessage>>
{
    public async Task Handle(HandleMessage<ProcessResearchFileCompletedMessage> request, CancellationToken cancellationToken)
    {
        var message = request.Message!;
        logger.LogInformation("Research file with Id: {Id} process completed", message.Id);

        using IServiceScope scope = serviceScopeFactory.CreateScope();
        IResearchFileRepository researchFileRepository =
            scope.ServiceProvider.GetRequiredService<IResearchFileRepository>();

        await researchFileRepository.SetSavedOnDiskAsync(message.Id, message.SavedLocally);
    }
}