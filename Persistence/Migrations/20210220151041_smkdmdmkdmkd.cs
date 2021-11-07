using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Migrations
{
    public partial class smkdmdmkdmkd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "item_discount",
                table: "NightLifeProducts",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<DateTime>(
                name: "order_date",
                table: "NightLifeOrder",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "BankCode",
                table: "DB_A57DC4_pnaDb_Vendor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order_date",
                table: "NightLifeOrder");

            migrationBuilder.DropColumn(
                name: "BankCode",
                table: "DB_A57DC4_pnaDb_Vendor");

            migrationBuilder.AlterColumn<double>(
                name: "item_discount",
                table: "NightLifeProducts",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}