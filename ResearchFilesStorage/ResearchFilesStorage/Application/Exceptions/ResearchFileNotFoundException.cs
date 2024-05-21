namespace ResearchFilesStorage.Application.Exceptions;

public class ResearchFileNotFoundException : Exception
{
    public ResearchFileNotFoundException(Guid id) : base($"ResearchFile with Id '{id}' not found.") { }
    public ResearchFileNotFoundException(string name, string? extension) : base($"ResearchFile with Name '{name}' and Extension {extension} not found.") { }
}