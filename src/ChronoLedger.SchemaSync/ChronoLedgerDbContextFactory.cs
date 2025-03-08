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
        
        Console.WriteLine(connectionString);
        
        optionsBuilder.UseNpgsql(connectionString);

        return new ChronoLedgerDbContext(optionsBuilder.Options);
    }
}