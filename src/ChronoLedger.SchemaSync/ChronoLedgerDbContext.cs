using ChronoLedger.Common.Cash;
using ChronoLedger.Schema.Accounts;
using ChronoLedger.Schema.Cash;
using ChronoLedger.Schema.Users;
using ChronoLedger.SchemaSync.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ChronoLedger.SchemaSync;

public class ChronoLedgerDbContext(DbContextOptions<ChronoLedgerDbContext> options) : DbContext(options)
{
    public DbSet<JournalBatchDto> JournalBatches { get; set; }
    
    public DbSet<UserDto> Users { get; set; }
    
    public DbSet<AccountDto> Accounts { get; set; }
    
    public DbSet<TransactionDto> Transactions { get; set; }
    
    public DbSet<TransactionNoteDto> TransactionNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureEnumTable<CurrencyCode>();

        ConfigureUser(modelBuilder);
        ConfigureJournalBatch(modelBuilder);
        ConfigureAccount(modelBuilder);
        ConfigureTransaction(modelBuilder);
        ConfigureTransactionNote(modelBuilder);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
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
    
    private static void ConfigureJournalBatch(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JournalBatchDto>()
            .HasOne<UserDto>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<JournalBatchDto>()
            .Property(x => x.CreatedDateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        modelBuilder.Entity<JournalBatchDto>()
            .Property(e => e.JournalsJson)
            .HasColumnType("json");
    }
    
    private void ConfigureAccount(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountDto>()
            .Property(x => x.AccountId)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("uuid_generate_v4()");
        
        modelBuilder.Entity<AccountDto>()
            .HasIndex(x => x.ExternalAccountId)
            .IsUnique();

        modelBuilder.Entity<AccountDto>()
            .Property(x => x.CreatedDateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
    }
    
    private void ConfigureTransaction(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionDto>()
            .HasOne<JournalBatchDto>()
            .WithMany()
            .HasForeignKey(x => x.JournalBatchId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<TransactionDto>()
            .HasOne<AccountDto>()
            .WithMany()
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<TransactionDto>()
            .Property(x => x.IsVisible)
            .HasDefaultValue(true);
        
        modelBuilder.Entity<TransactionDto>()
            .Property(x => x.Amount)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<TransactionDto>()
            .Property(x => x.CurrencyCodeId)
            .HasConversion<int>();
        
        modelBuilder.ConfigureEnumRelationship<TransactionDto, CurrencyCode>(x => x.CurrencyCodeId);
    }

    private void ConfigureTransactionNote(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionNoteDto>()
            .HasOne<TransactionDto>()
            .WithOne()
            .HasForeignKey<TransactionNoteDto>(x => x.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<TransactionNoteDto>()
            .Property(x => x.Note)
            .HasMaxLength(255);
    }


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Remove<ForeignKeyIndexConvention>();
    }
}
