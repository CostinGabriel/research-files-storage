using MediatR;
using ResearchFilesStorage.Application.Enums;
using ResearchFilesStorage.Domain.Repository;

namespace ResearchFilesStorage.Application.Commands;


public record AddResearchFileCommand(string Name, string Content, Extension Extension) : IRequest<Guid>;

public class AddResearchFileCommandHandler(
    IResearchFileRepository researchFileRepository,
    IFileBuilder fileBuilder) : IRequestHandler<AddResearchFileCommand, Guid>
{
    public async Task<Guid> Handle(AddResearchFileCommand request, CancellationToken cancellationToken)
    {
        var researchFile = fileBuilder.SetName(request.Name)
                                      .SetContent(request.Content)
                                      .SetExtension(request.Extension)
                                      .Build();

        var id = await researchFileRepository.AddAsync(researchFile);

        return id;
    }
}
