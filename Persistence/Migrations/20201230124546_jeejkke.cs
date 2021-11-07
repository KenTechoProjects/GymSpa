using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Migrations
{
    public partial class jeejkke : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivationCode",
                table: "DB_A57DC4_pnaDb_Member",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "DB_A57DC4_pnaDb_Member",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DB_A57DC4_pnaDb_Member",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FundWalletHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    paymentReference = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    IsWalletFunded = table.Column<bool>(nullable: false),
                    paymentMethod = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    createdOn = table.Column<DateTime>(nullable: true),
                    amount = table.Column<double>(nullable: false),
                    currencyCode = table.Column<string>(nullable: true),
                    completedOn = table.Column<DateTime>(nullable: true),
                    customerName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    customerEmail = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    paymentDescription = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    paymentStatus = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    transactionReference = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    payableAmount = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    amountPaid = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    completed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundWalletHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembershipSubcription",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    paymentReference = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    IsMembershipsubscribed = table.Column<bool>(nullable: false),
                    MemberType = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    paymentMethod = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    createdOn = table.Column<DateTime>(nullable: true),
                    amount = table.Column<double>(nullable: false),
                    currencyCode = table.Column<string>(nullable: true),
                    completedOn = table.Column<DateTime>(nullable: true),
                    customerName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    customerEmail = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    paymentDescription = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    paymentStatus = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    transactionReference = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    payableAmount = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    amountPaid = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    completed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipSubcription", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundWalletHistory");

            migrationBuilder.DropTable(
                name: "MembershipSubcription");

            migrationBuilder.DropColumn(
                name: "ActivationCode",
                table: "DB_A57DC4_pnaDb_Member");

            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "DB_A57DC4_pnaDb_Member");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DB_A57DC4_pnaDb_Member");
        }
    }
}