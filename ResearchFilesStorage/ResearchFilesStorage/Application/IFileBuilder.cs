using ResearchFilesStorage.Application.Enums;
using ResearchFilesStorage.Domain.Builders;
using ResearchFilesStorage.Domain.Entities;

namespace ResearchFilesStorage.Application;

public interface IFileBuilder
{
    ResearchFile Build();
    FileBuilder SetId(Guid id);
    FileBuilder SetContent(string content);
    FileBuilder SetExtension(Extension extension);
    FileBuilder SetIsSecured(bool isSecured);
    FileBuilder SetName(string name);
    FileBuilder SetPassword(string password);
}