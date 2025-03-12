using ChronoLedger.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ChronoLedger.SchemaSync;

public class ChronoLedgerDbContextFactory : IDesignTimeDbContextFactory<ChronoLedgerDbContext>
{
    public ChronoLedgerDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ChronoLedgerDbContext>();
        const string connectionStringEnvironmentVariableName = "SchemaSyncSqlConnectionString";
        var connectionString = configuration[connectionStringEnvironmentVariableName];
        
        if (connectionString.IsNullOrEmpty())
        {
            throw new Exception($"Could not find {connectionStringEnvironmentVariableName} environment variable");
        }
        
        optionsBuilder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();
        
        return new ChronoLedgerDbContext(optionsBuilder.Options);
    }
}