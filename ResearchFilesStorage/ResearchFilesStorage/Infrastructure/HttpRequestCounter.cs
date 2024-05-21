namespace ResearchFilesStorage.Infrastructure;

public sealed class HttpRequestCounter
{
    private long _counter = 0;
    private static readonly object _instancelock = new object();
    private static readonly HttpRequestCounter _instance = new HttpRequestCounter();

    static HttpRequestCounter()
    {
    }

    private HttpRequestCounter()
    {
    }

    public static HttpRequestCounter Instance
    {
        get { return _instance; }
    }

    public long GetCount() => _counter;

    public void IncreaseCounter()
    {
        // Interlocked class could also be used in this case
        lock (_instancelock)
        {
            _counter++;
        }
    }
}
