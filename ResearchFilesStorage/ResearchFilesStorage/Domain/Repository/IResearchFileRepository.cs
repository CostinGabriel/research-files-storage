using ResearchFilesStorage.Domain.Entities;

namespace ResearchFilesStorage.Domain.Repository;

public interface IResearchFileRepository
{
    Task<ResearchFile> GetByIdAsync(Guid id);
    Task<ResearchFile> GetByNameAsync(string name, string? extension);
    Task<Guid> AddAsync(ResearchFile researchFile);
    Task UpdateAsync(ResearchFile researchFile);
    Task SetSavedOnDiskAsync(Guid id, bool saved);
    Task RemoveAsync(Guid id);
}
