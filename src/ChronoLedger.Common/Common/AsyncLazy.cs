namespace ChronoLedger.Common.Common;

public class AsyncLazy<T>(Func<Task<T>> factory)
{
    private readonly Lazy<Task<T>> _instance = new(factory);

    public Task<T> ValueAsync => _instance.Value;
}
