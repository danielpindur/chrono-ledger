using ChronoLedger.Common.Common;

namespace ChronoLedger.Common.Database;

public interface IDatabaseContextProvider
{
    Task<IDatabaseContext> GetContextAsync();
}

internal class DatabaseContextProvider : IDatabaseContextProvider
{
    private readonly AsyncLazy<IDatabaseContext> _dbContext;
    
    public DatabaseContextProvider(IDbConnectionFactory connectionFactory)
    {
        _dbContext = new AsyncLazy<IDatabaseContext>(() => 
            DatabaseContext.CreateAsync(connectionFactory));
    }
    
    public Task<IDatabaseContext> GetContextAsync() => _dbContext.ValueAsync;
}