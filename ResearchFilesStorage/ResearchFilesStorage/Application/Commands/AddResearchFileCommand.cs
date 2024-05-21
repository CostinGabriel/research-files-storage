using MediatR;
using RabbitMq.Messages;
using ResearchFilesStorage.Application.Enums;
using ResearchFilesStorage.Domain.Repository;
using ResearchFilesStorage.RabbitMq;

namespace ResearchFilesStorage.Application.Commands;


public record AddResearchFileCommand(string Name, string Content, Extension Extension) : IRequest<Guid>;

public class AddResearchFileCommandHandler(
    IResearchFileRepository researchFileRepository,
    IFileBuilder fileBuilder,
    IProcessFileMessageQueueProducer messageProducer) : IRequestHandler<AddResearchFileCommand, Guid>
{
    public async Task<Guid> Handle(AddResearchFileCommand request, CancellationToken cancellationToken)
    {
        var researchFile = fileBuilder.SetName(request.Name)
                                      .SetContent(request.Content)
                                      .SetExtension(request.Extension)
                                      .Build();

        var id = await researchFileRepository.AddAsync(researchFile);

        var message = new ProcessResearchFileMessage()
        {
            Id = id,
            Content = researchFile.Content,
            Extension = researchFile.Extension,
            Name = researchFile.Name
        };

        messageProducer.Send(message);

        return id;
    }
}