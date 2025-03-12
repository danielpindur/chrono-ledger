using System.Data;
using ChronoLedger.Common.Database;
using Npgsql;

namespace ChronoLedger.Database;

internal class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory()
    {
        _connectionString = Environment.GetEnvironmentVariable("DatabaseSqlConnectionString")
                            ?? throw new InvalidOperationException(
                                "Environment variable 'DatabaseSqlConnectionString' not set.");
    }
    
    public IDbConnection Create()
    {
        return new NpgsqlConnection(_connectionString);
    }
}