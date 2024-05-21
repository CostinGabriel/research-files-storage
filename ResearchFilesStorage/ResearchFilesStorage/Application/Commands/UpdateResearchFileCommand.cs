using MediatR;

namespace ResearchFilesStorage.Application.Commands;

public record UpdateResearchFileCommand(Guid Id, string Name, string Content, string? Password) : IRequest;

public class UpdateResearchFileCommandHandler() : IRequestHandler<UpdateResearchFileCommand>
{
    public async Task Handle(UpdateResearchFileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
