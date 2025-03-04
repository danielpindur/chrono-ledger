using ChronoLedger.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChronoLedger.SchemaSync;

public class ChronoLedgerDbContext(DbContextOptions<ChronoLedgerDbContext> options) : DbContext(options)
{
    public DbSet<JournalBatch> JournalBatches { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JournalBatch>()
            .Property(e => e.JournalsJson)
            .HasColumnType("json");
    }
}
