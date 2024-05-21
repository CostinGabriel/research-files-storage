using MediatR;
using ResearchFilesStorage.Domain.Constants;

namespace ResearchFilesStorage.Application.Commands;


public record AddResearchFileCommand(string Name, string Content, Extension Extension) : IRequest<Guid>;

public class AddResearchFileCommandHandler() : IRequestHandler<AddResearchFileCommand, Guid>
{
    public async Task<Guid> Handle(AddResearchFileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}