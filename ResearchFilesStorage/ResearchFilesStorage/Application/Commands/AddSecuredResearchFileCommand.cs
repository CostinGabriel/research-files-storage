using MediatR;
using ResearchFilesStorage.Domain.Constants;

namespace ResearchFilesStorage.Application.Commands;

public record AddSecuredResearchFileCommand(string Name, string Content, Extension Extension, string Password) : IRequest<Guid>;

public class AddSecuredResearchFileCommandHandler() : IRequestHandler<AddSecuredResearchFileCommand, Guid>
{
    public async Task<Guid> Handle(AddSecuredResearchFileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}