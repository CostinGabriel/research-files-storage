using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ResearchFilesStorage.Domain.Entities;
using ResearchFilesStorage.Domain.Repository;

namespace ResearchFilesStorage.Infrastructure;

public class ResearchFileRepository : IResearchFileRepository
{
    private readonly IMongoCollection<ResearchFile> _researchFileCollection;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ResearchFileRepository(IOptionsSnapshot<ResearchFileDatabaseSettings> options, IDateTimeProvider dateTimeProvider)
    {
        var db = new MongoClient(options.Value.ConnectionString)
            .GetDatabase(options.Value.DatabaseName);

        _researchFileCollection = db.GetCollection<ResearchFile>("ResearchFiles");
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ResearchFile> GetByIdAsync(Guid id)
        => await _researchFileCollection.Find(researchFile => researchFile.Id == id)
                                        .FirstOrDefaultAsync();

    public async Task<ResearchFile> GetByNameAsync(string name, string? extension)
        => await _researchFileCollection.Find(researchFile => researchFile.Name == name && researchFile.Extension == extension)
                                        .FirstOrDefaultAsync();

    public async Task<Guid> AddAsync(ResearchFile researchFile)
    {
        researchFile.CreatedAt = _dateTimeProvider.UtcNow();
        await _researchFileCollection.InsertOneAsync(researchFile);
        return researchFile.Id;
    }

    public async Task UpdateAsync(ResearchFile researchFile)
    {
        var filter = new FilterDefinitionBuilder<ResearchFile>().Eq(r => r.Id, researchFile.Id);

        var update = Builders<ResearchFile>.Update
            .Set(r => r.Content, researchFile.Content)
            .Set(r => r.Name, researchFile.Name)
            .Set(r => r.LastUpdatedAt, _dateTimeProvider.UtcNow());

        await _researchFileCollection.UpdateOneAsync(filter, update);
    }

    public async Task SetSavedOnDiskAsync(Guid id, bool saved)
    {
        var filter = new FilterDefinitionBuilder<ResearchFile>().Eq(r => r.Id, id);

        var update = Builders<ResearchFile>.Update
            .Set(r => r.SavedOnLocalDisk, saved)
            .Set(r => r.LastUpdatedAt, _dateTimeProvider.UtcNow());

        await _researchFileCollection.UpdateOneAsync(filter, update);
    }

    public async Task RemoveAsync(Guid id)
        => await _researchFileCollection.DeleteOneAsync(x => x.Id == id);
}