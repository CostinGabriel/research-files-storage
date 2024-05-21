using ResearchFilesStorage.Application;
using ResearchFilesStorage.Application.Enums;
using ResearchFilesStorage.Domain.Entities;

namespace ResearchFilesStorage.Domain.Builders;

public class FileBuilder : IFileBuilder
{
    private readonly ResearchFile _file;

    public FileBuilder() => _file = new();

    public FileBuilder SetId(Guid id)
    {
        _file.Id = id;
        return this;
    }

    public FileBuilder SetName(string name)
    {
        _file.Name = name;
        return this;
    }

    public FileBuilder SetContent(string content)
    {
        _file.Content = content;
        return this;
    }

    public FileBuilder SetExtension(Extension extension)
    {
        _file.Extension = extension.GetDescription();
        return this;
    }

    public FileBuilder SetPassword(string password)
    {
        _file.Password = password;
        return this;
    }

    public FileBuilder SetIsSecured(bool isSecured)
    {
        _file.IsSecured = isSecured;
        return this;
    }

    public ResearchFile Build()
    {
        return _file;
    }
}
