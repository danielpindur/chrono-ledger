using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChronoLedger.SchemaSync.Migrations
{
    /// <inheritdoc />
    public partial class Transaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "journals_json",
                table: "journal_batches",
                type: "json",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    external_account_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounts", x => x.account_id);
                });

            migrationBuilder.CreateTable(
                name: "currency_codes",
                columns: table => new
                {
                    currency_code_id = table.Column<byte>(type: "smallint", nullable: false),
                    currency_code_name = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currency_codes", x => x.currency_code_id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    transaction_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    journal_batch_id = table.Column<long>(type: "bigint", nullable: false),
                    ledger_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_visible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    currency_code_id = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.transaction_id);
                    table.ForeignKey(
                        name: "fk_transactions_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transactions_currency_codes_currency_code_id",
                        column: x => x.currency_code_id,
                        principalTable: "currency_codes",
                        principalColumn: "currency_code_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transactions_journal_batches_journal_batch_id",
                        column: x => x.journal_batch_id,
                        principalTable: "journal_batches",
                        principalColumn: "journal_batch_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_notes",
                columns: table => new
                {
                    transaction_id = table.Column<long>(type: "bigint", nullable: false),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transaction_notes", x => x.transaction_id);
                    table.ForeignKey(
                        name: "fk_transaction_notes_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "transaction_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "currency_codes",
                columns: new[] { "currency_code_id", "currency_code_name" },
                values: new object[] { (byte)1, "USD" });

            migrationBuilder.CreateIndex(
                name: "ix_accounts_external_account_id",
                table: "accounts",
                column: "external_account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currency_codes_currency_code_name",
                table: "currency_codes",
                column: "currency_code_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transaction_notes");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "currency_codes");

            migrationBuilder.DropColumn(
                name: "journals_json",
                table: "journal_batches");
        }
    }
}
