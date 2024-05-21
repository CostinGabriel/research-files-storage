using MediatR;
using ResearchFilesStorage.Domain.Entities;

namespace ResearchFilesStorage.Application.Queries;

public record GetResearchFileByIdQuery(Guid Id) : IRequest<ResearchFile>;

public class GetResearchFileByIdQueryHandler() : IRequestHandler<GetResearchFileByIdQuery, ResearchFile>
{
    public async Task<ResearchFile> Handle(GetResearchFileByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}