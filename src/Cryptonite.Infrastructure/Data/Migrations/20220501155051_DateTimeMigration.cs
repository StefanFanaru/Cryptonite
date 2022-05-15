using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cryptonite.Infrastructure.Data.Migrations
{
    public partial class DateTimeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TradedAt",
                schema: "cryptonite",
                table: "TradeEntries",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BoughtAt",
                schema: "cryptonite",
                table: "BuyEntries",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TradedAt",
                schema: "cryptonite",
                table: "TradeEntries",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BoughtAt",
                schema: "cryptonite",
                table: "BuyEntries",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
