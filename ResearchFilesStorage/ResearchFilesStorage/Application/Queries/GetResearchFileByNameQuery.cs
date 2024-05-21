using MediatR;
using ResearchFilesStorage.Domain.Constants;
using ResearchFilesStorage.Domain.Entities;

namespace ResearchFilesStorage.Application.Queries;

public record GetResearchFileByNameQuery(string Name, Extension Extension) : IRequest<ResearchFile>;

public class GetResearchFileByNameQueryHandler() : IRequestHandler<GetResearchFileByNameQuery, ResearchFile>
{
    public async Task<ResearchFile> Handle(GetResearchFileByNameQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}