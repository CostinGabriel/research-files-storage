namespace ResearchFilesStorage.Application.Validation;

public record ValidationError(string PropertyName, string ErrorMessage);
