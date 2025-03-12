namespace ChronoLedger.Common.Database;

public class TransactionAndConnectionScope : IDisposable
{
    private readonly IDatabaseContext _databaseContext;
    private bool _completed;
    private bool _disposed;
    
    public TransactionAndConnectionScope(IServiceProvider serviceProvider)
    {
        var databaseContext = serviceProvider.GetService(typeof(IDatabaseContext)) as IDatabaseContext;

        _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        _databaseContext.BeginTransaction();
    }
    
    public void Commit()
    {
        if (!_disposed)
        {
            _databaseContext.Commit();
            _completed = true;
        }
        else
        {
            throw new ObjectDisposedException(nameof(TransactionAndConnectionScope));
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (!_completed)
        {
            _databaseContext.Rollback();
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}