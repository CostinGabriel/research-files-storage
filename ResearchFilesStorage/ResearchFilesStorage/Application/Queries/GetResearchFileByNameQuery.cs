using MediatR;
using ResearchFilesStorage.Application.Enums;
using ResearchFilesStorage.Application.Exceptions;
using ResearchFilesStorage.Domain.Entities;
using ResearchFilesStorage.Domain.Repository;

namespace ResearchFilesStorage.Application.Queries;

public record GetResearchFileByNameQuery(string Name, Extension Extension) : IRequest<ResearchFile>;

public class GetResearchFileByNameQueryHandler(
    IResearchFileRepository researchFileRepository) : IRequestHandler<GetResearchFileByNameQuery, ResearchFile>
{
    public async Task<ResearchFile> Handle(GetResearchFileByNameQuery request, CancellationToken cancellationToken)
    {
        var researchFile = await researchFileRepository.GetByNameAsync(request.Name, request.Extension.GetDescription());

        if (researchFile is null)
            throw new ResearchFileNotFoundException(request.Name, request.Extension.GetDescription());

        return researchFile;
    }
}