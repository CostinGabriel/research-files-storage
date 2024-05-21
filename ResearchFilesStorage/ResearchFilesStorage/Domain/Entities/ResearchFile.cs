using MongoDB.Bson.Serialization.Attributes;

namespace ResearchFilesStorage.Domain.Entities;

public class ResearchFile
{
    [BsonId]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public string? Extension { get; set; }
    public string? Password { get; set; }
    public bool IsSecured { get; set; }
    public bool SavedOnLocalDisk { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
}
