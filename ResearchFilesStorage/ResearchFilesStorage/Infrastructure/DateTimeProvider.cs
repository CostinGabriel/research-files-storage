namespace ResearchFilesStorage.Infrastructure;

public interface IDateTimeProvider
{
    DateTime UtcNow();
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow()
    {
        return DateTime.UtcNow;
    }
}
