using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Migrations
{
    public partial class dkmkm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WalletTransferHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderFullName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    SenderWalletId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    SenderPhone = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    SenderEmail = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ReceiverFullName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ReceiverWalletId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ReceiverPhone = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Narration = table.Column<string>(nullable: true),
                    TransactionReference = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    Transactionstatus = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransferHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletTransferHistory");
        }
    }
}