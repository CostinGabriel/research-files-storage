using MediatR;
using RabbitMq.Messages;
using ResearchFilesStorage.Application.Exceptions;
using ResearchFilesStorage.Domain.Repository;
using ResearchFilesStorage.RabbitMq;

namespace ResearchFilesStorage.Application.Commands;

public record UpdateResearchFileCommand(Guid Id, string Name, string Content, string? Password) : IRequest;

public class UpdateResearchFileCommandHandler(
    IResearchFileRepository researchFileRepository,
    IFileBuilder fileBuilder,
    IProcessFileMessageQueueProducer messageProducer) : IRequestHandler<UpdateResearchFileCommand>
{
    public async Task Handle(UpdateResearchFileCommand request, CancellationToken cancellationToken)
    {
        var researchFile = await researchFileRepository.GetByIdAsync(request.Id);

        if (researchFile is null)
            throw new ResearchFileNotFoundException(request.Id);

        if (researchFile.IsSecured && researchFile.Password != request.Password)
            throw new InvalidPasswordException();

        var updatedResearchFile = fileBuilder.SetId(request.Id)
                                             .SetName(request.Name)
                                             .SetContent(request.Content)
                                             .Build();

        await researchFileRepository.UpdateAsync(updatedResearchFile);

        var message = new ProcessResearchFileMessage()
        {
            Id = request.Id,
            Content = request.Content,
            Extension = researchFile.Extension,
            Name = request.Name
        };

        messageProducer.Send(message);
    }
}
