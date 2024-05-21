namespace ResearchFilesStorage.Application.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base($"Cannot update secured file. Password not matching.") { }
}

