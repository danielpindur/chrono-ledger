using ChronoLedger.Schema.Journals;
using ChronoLedger.Schema.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ChronoLedger.SchemaSync;

public class ChronoLedgerDbContext(DbContextOptions<ChronoLedgerDbContext> options) : DbContext(options)
{
    public DbSet<JournalBatch> JournalBatches { get; set; }
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JournalBatch>()
            .Property(e => e.JournalsJson)
            .HasColumnType("json");

        modelBuilder.Entity<JournalBatch>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<JournalBatch>()
            .Property(x => x.CreatedDateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
        
        modelBuilder.Entity<User>()
            .Property(x => x.ExternalUserId)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.ExternalUserId)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(x => x.CreatedDateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Remove<ForeignKeyIndexConvention>();
    }
}
