using MediatR;
using ResearchFilesStorage.Application.Exceptions;
using ResearchFilesStorage.Domain.Entities;
using ResearchFilesStorage.Domain.Repository;

namespace ResearchFilesStorage.Application.Queries;

public record GetResearchFileByIdQuery(Guid Id) : IRequest<ResearchFile>;

public class GetResearchFileByIdQueryHandler(
    IResearchFileRepository researchFileRepository) : IRequestHandler<GetResearchFileByIdQuery, ResearchFile>
{
    public async Task<ResearchFile> Handle(GetResearchFileByIdQuery request, CancellationToken cancellationToken)
    {
        var researchFile = await researchFileRepository.GetByIdAsync(request.Id);

        if (researchFile is null)
            throw new ResearchFileNotFoundException(request.Id);

        return researchFile;
    }
}