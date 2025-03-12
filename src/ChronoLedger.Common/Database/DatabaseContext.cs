using System.Data;

namespace ChronoLedger.Common.Database;

public interface IDatabaseContext : IDisposable
{
    IDbConnection Connection { get; }
    
    IDbTransaction? Transaction { get; }

    void BeginTransaction();

    void Commit();

    void Rollback();
}

public class DatabaseContext : IDatabaseContext
{
    private readonly IDbConnection _connection;
    private bool _disposed;
    
    public DatabaseContext(IDbConnectionFactory connectionFactory)
    {
        _connection = connectionFactory.Create();
        _connection.Open();
    }   
    
    public IDbConnection Connection => _connection;
    
    public IDbTransaction? Transaction { get; private set; }

    public void BeginTransaction()
    {
        Transaction ??= _connection.BeginTransaction();
    }

    public void Commit()
    {
        Transaction?.Commit();
        Transaction?.Dispose();
        Transaction = null;
    }

    public void Rollback()
    {
        Transaction?.Rollback();
        Transaction?.Dispose();
        Transaction = null;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        
        Transaction?.Dispose();
        _connection.Dispose();
        _disposed = true;
    }
}