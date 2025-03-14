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
    [Migration("20250311201405_JournalBatch")]
    partial class JournalBatch
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ChronoLedger.Schema.Journals.JournalBatchDto", b =>
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

                    b.HasKey("JournalBatchId")
                        .HasName("pk_journal_batches");

                    b.ToTable("journal_batches", (string)null);
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

            modelBuilder.Entity("ChronoLedger.Schema.Journals.JournalBatchDto", b =>
                {
                    b.HasOne("ChronoLedger.Schema.Users.UserDto", null)
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_journal_batches_users_created_by_user_id");
                });
#pragma warning restore 612, 618
        }
    }
}
