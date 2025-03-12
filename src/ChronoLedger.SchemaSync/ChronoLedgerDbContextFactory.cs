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
        var connectionString = configuration["SchemaSyncSqlConnectionString"];
        
        optionsBuilder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();
        
        return new ChronoLedgerDbContext(optionsBuilder.Options);
    }
}