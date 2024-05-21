using MediatR;

namespace ResearchFilesStorage.Application.Commands;

public record RemoveResearchFileCommand(Guid Id) : IRequest;

public class RemoveResearchFileCommandHandler() : IRequestHandler<RemoveResearchFileCommand>
{
    public async Task Handle(RemoveResearchFileCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}