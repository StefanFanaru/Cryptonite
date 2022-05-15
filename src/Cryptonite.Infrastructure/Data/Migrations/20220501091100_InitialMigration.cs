using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cryptonite.Infrastructure.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cryptonite");

            migrationBuilder.CreateTable(
                name: "BuyEntries",
                schema: "cryptonite",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    BoughtAmount = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    PaymentCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    BoughtCryptocurrency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BoughtAt = table.Column<DateTime>(type: "date", nullable: false),
                    PaidUsd = table.Column<decimal>(type: "decimal(18,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "cryptonite",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Currencies = table.Column<byte[]>(type: "varbinary(5000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "DataMigrations",
                schema: "cryptonite",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataMigrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Portofolios",
                schema: "cryptonite",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Transactions = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastTransactionAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portofolios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeEntries",
                schema: "cryptonite",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    GainedAmount = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    PaidCryptocurrency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GainedCryptocurrency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TradedAt = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                schema: "cryptonite",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PreferredCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    BankAccountCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankConversionMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0.0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "PortofolioCryptocurrencies",
                schema: "cryptonite",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PortofolioId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsertedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortofolioCryptocurrencies", x => new { x.PortofolioId, x.Symbol });
                    table.ForeignKey(
                        name: "FK_PortofolioCryptocurrencies_Portofolios_PortofolioId",
                        column: x => x.PortofolioId,
                        principalSchema: "cryptonite",
                        principalTable: "Portofolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyEntries_UserId",
                schema: "cryptonite",
                table: "BuyEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Portofolios_UserId",
                schema: "cryptonite",
                table: "Portofolios",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeEntries_UserId",
                schema: "cryptonite",
                table: "TradeEntries",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyEntries",
                schema: "cryptonite");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "cryptonite");

            migrationBuilder.DropTable(
                name: "DataMigrations",
                schema: "cryptonite");

            migrationBuilder.DropTable(
                name: "PortofolioCryptocurrencies",
                schema: "cryptonite");

            migrationBuilder.DropTable(
                name: "TradeEntries",
                schema: "cryptonite");

            migrationBuilder.DropTable(
                name: "UserSettings",
                schema: "cryptonite");

            migrationBuilder.DropTable(
                name: "Portofolios",
                schema: "cryptonite");
        }
    }
}
