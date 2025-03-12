using Microsoft.Extensions.DependencyInjection;

namespace ChronoLedger.Common.Database;

public class TransactionAndConnectionScope : IAsyncDisposable
{
    private readonly IDatabaseContext _databaseContext;
    private bool _completed;
    private bool _disposed;

    public TransactionAndConnectionScope(IServiceProvider serviceProvider)
    {
        _databaseContext = serviceProvider.GetRequiredService<IDatabaseContext>();
    }

    public async Task BeginAsync()
    {
        await _databaseContext.BeginTransactionAsync().ConfigureAwait(false);
    }

    public async Task CommitAsync()
    {
        if (!_disposed)
        {
            await _databaseContext.CommitAsync().ConfigureAwait(false);
            _completed = true;
        }
        else
        {
            throw new ObjectDisposedException(nameof(TransactionAndConnectionScope));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (!_completed)
        {
            await _databaseContext.RollbackAsync().ConfigureAwait(false);
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}