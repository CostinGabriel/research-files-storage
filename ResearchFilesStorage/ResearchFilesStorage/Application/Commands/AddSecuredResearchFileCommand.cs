using MediatR;
using ResearchFilesStorage.Application.Enums;
using ResearchFilesStorage.Domain.Repository;

namespace ResearchFilesStorage.Application.Commands;

public record AddSecuredResearchFileCommand(string Name, string Content, Extension Extension, string Password) : IRequest<Guid>;

public class AddSecuredResearchFileCommandHandler(
    IResearchFileRepository researchFileRepository,
    IFileBuilder fileBuilder) : IRequestHandler<AddSecuredResearchFileCommand, Guid>
{
    public async Task<Guid> Handle(AddSecuredResearchFileCommand request, CancellationToken cancellationToken)
    {
        var researchFile = fileBuilder.SetName(request.Name)
                                      .SetContent(request.Content)
                                      .SetExtension(request.Extension)
                                      .SetPassword(request.Password)
                                      .SetIsSecured(true)
                                      .Build();

        var id = await researchFileRepository.AddAsync(researchFile);

        return id;
    }
}
