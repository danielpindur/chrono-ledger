using System.Data.Common;

namespace ChronoLedger.Common.Database;

public interface IDatabaseContext : IAsyncDisposable
{
    DbConnection Connection { get; }
    
    DbTransaction? Transaction { get; }

    Task BeginTransactionAsync();

    Task CommitAsync();

    Task RollbackAsync();
}

internal class DatabaseContext : IDatabaseContext
{
    private readonly DbConnection _connection;
    private bool _disposed;
    
    private DatabaseContext(DbConnection connection)
    {
        _connection = connection;
    }

    public DbConnection Connection => _connection;
    
    public DbTransaction? Transaction { get; private set; }

    public static async Task<IDatabaseContext> CreateAsync(IDbConnectionFactory connectionFactory)
    {
        var connection = connectionFactory.Create();
        await connection.OpenAsync().ConfigureAwait(false);
        return new DatabaseContext(connection);
    }
    
    public async Task BeginTransactionAsync()
    {
        Transaction ??= await _connection.BeginTransactionAsync().ConfigureAwait(false);
    }

    public async Task CommitAsync()
    {
        if (Transaction is null)
        {
            return;
        }
        
        await Transaction.CommitAsync().ConfigureAwait(false);
        await Transaction.DisposeAsync().ConfigureAwait(false);
        Transaction = null;
    }

    public async Task RollbackAsync()
    {
        if (Transaction is null)
        {
            return;
        }
        
        await Transaction.RollbackAsync().ConfigureAwait(false);
        await Transaction.DisposeAsync().ConfigureAwait(false);
        Transaction = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }
        
        if (Transaction is not null)
        {
            await Transaction.DisposeAsync().ConfigureAwait(false);
        }
        
        await _connection.DisposeAsync().ConfigureAwait(false);
        _disposed = true;
    }
}