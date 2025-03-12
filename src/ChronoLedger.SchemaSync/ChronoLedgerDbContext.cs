using ChronoLedger.Schema.Journals;
using ChronoLedger.Schema.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ChronoLedger.SchemaSync;

public class ChronoLedgerDbContext(DbContextOptions<ChronoLedgerDbContext> options) : DbContext(options)
{
    public DbSet<JournalBatchDto> JournalBatches { get; set; }
    
    public DbSet<UserDto> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*
        modelBuilder.Entity<JournalBatchDto>()
            .Property(e => e.JournalsJson)
            .HasColumnType("json");
        */
        
        modelBuilder.Entity<JournalBatchDto>()
            .HasOne<UserDto>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<JournalBatchDto>()
            .Property(x => x.CreatedDateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
        
        modelBuilder.Entity<UserDto>()
            .Property(x => x.UserId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("uuid_generate_v4()");
        
        modelBuilder.Entity<UserDto>()
            .Property(x => x.ExternalUserId)
            .HasMaxLength(50);

        modelBuilder.Entity<UserDto>()
            .HasIndex(x => x.ExternalUserId)
            .IsUnique();

        modelBuilder.Entity<UserDto>()
            .Property(x => x.CreatedDateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Remove<ForeignKeyIndexConvention>();
    }
}
