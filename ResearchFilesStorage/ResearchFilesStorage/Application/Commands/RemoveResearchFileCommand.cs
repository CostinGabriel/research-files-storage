using MediatR;
using ResearchFilesStorage.Domain.Repository;

namespace ResearchFilesStorage.Application.Commands;

public record RemoveResearchFileCommand(Guid Id) : IRequest;

public class RemoveResearchFileCommandHandler(
    IResearchFileRepository researchFileRepository) : IRequestHandler<RemoveResearchFileCommand>
{
    public async Task Handle(RemoveResearchFileCommand request, CancellationToken cancellationToken)
        => await researchFileRepository.RemoveAsync(request.Id);
}