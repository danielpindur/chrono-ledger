﻿// <auto-generated />
using System;
using ChronoLedger.SchemaSync;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChronoLedger.SchemaSync.Migrations
{
    [DbContext(typeof(ChronoLedgerDbContext))]
    [Migration("20250312214703_Transaction")]
    partial class Transaction
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ChronoLedger.Common.Cash.CurrencyCodeEntity", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint")
                        .HasColumnName("currency_code_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("currency_code_name");

                    b.HasKey("Id")
                        .HasName("pk_currency_codes");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_currency_codes_currency_code_name");

                    b.ToTable("currency_codes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            Name = "USD"
                        });
                });

            modelBuilder.Entity("ChronoLedger.Schema.Accounts.AccountDto", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("account_id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

                    b.Property<string>("ExternalAccountId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("external_account_id");

                    b.HasKey("AccountId")
                        .HasName("pk_accounts");

                    b.HasIndex("ExternalAccountId")
                        .IsUnique()
                        .HasDatabaseName("ix_accounts_external_account_id");

                    b.ToTable("accounts", (string)null);
                });

            modelBuilder.Entity("ChronoLedger.Schema.Cash.JournalBatchDto", b =>
                {
                    b.Property<long>("JournalBatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("journal_batch_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("JournalBatchId"));

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by_user_id");

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

                    b.Property<string>("JournalsJson")
                        .IsRequired()
                        .HasColumnType("json")
                        .HasColumnName("journals_json");

                    b.HasKey("JournalBatchId")
                        .HasName("pk_journal_batches");

                    b.ToTable("journal_batches", (string)null);
                });

            modelBuilder.Entity("ChronoLedger.Schema.Cash.TransactionDto", b =>
                {
                    b.Property<long>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("transaction_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("TransactionId"));

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("amount");

                    b.Property<byte>("CurrencyCodeId")
                        .HasColumnType("smallint")
                        .HasColumnName("currency_code_id");

                    b.Property<bool>("IsVisible")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("is_visible");

                    b.Property<long>("JournalBatchId")
                        .HasColumnType("bigint")
                        .HasColumnName("journal_batch_id");

                    b.Property<DateOnly>("LedgerDate")
                        .HasColumnType("date")
                        .HasColumnName("ledger_date");

                    b.HasKey("TransactionId")
                        .HasName("pk_transactions");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("ChronoLedger.Schema.Cash.TransactionNoteDto", b =>
                {
                    b.Property<long>("TransactionId")
                        .HasColumnType("bigint")
                        .HasColumnName("transaction_id");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("note");

                    b.HasKey("TransactionId")
                        .HasName("pk_transaction_notes");

                    b.ToTable("transaction_notes", (string)null);
                });

            modelBuilder.Entity("ChronoLedger.Schema.Users.UserDto", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

                    b.Property<string>("ExternalUserId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("external_user_id");

                    b.HasKey("UserId")
                        .HasName("pk_users");

                    b.HasIndex("ExternalUserId")
                        .IsUnique()
                        .HasDatabaseName("ix_users_external_user_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("ChronoLedger.Schema.Cash.JournalBatchDto", b =>
                {
                    b.HasOne("ChronoLedger.Schema.Users.UserDto", null)
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_journal_batches_users_created_by_user_id");
                });

            modelBuilder.Entity("ChronoLedger.Schema.Cash.TransactionDto", b =>
                {
                    b.HasOne("ChronoLedger.Schema.Accounts.AccountDto", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_accounts_account_id");

                    b.HasOne("ChronoLedger.Common.Cash.CurrencyCodeEntity", null)
                        .WithMany()
                        .HasForeignKey("CurrencyCodeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_currency_codes_currency_code_id");

                    b.HasOne("ChronoLedger.Schema.Cash.JournalBatchDto", null)
                        .WithMany()
                        .HasForeignKey("JournalBatchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_journal_batches_journal_batch_id");
                });

            modelBuilder.Entity("ChronoLedger.Schema.Cash.TransactionNoteDto", b =>
                {
                    b.HasOne("ChronoLedger.Schema.Cash.TransactionDto", null)
                        .WithOne()
                        .HasForeignKey("ChronoLedger.Schema.Cash.TransactionNoteDto", "TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transaction_notes_transactions_transaction_id");
                });
#pragma warning restore 612, 618
        }
    }
}
