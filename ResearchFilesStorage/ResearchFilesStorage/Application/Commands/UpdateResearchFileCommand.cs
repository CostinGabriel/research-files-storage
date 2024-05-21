using MediatR;
using ResearchFilesStorage.Application.Exceptions;
using ResearchFilesStorage.Domain.Repository;

namespace ResearchFilesStorage.Application.Commands;

public record UpdateResearchFileCommand(Guid Id, string Name, string Content, string? Password) : IRequest;

public class UpdateResearchFileCommandHandler(
    IResearchFileRepository researchFileRepository,
    IFileBuilder fileBuilder) : IRequestHandler<UpdateResearchFileCommand>
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
    }
}
