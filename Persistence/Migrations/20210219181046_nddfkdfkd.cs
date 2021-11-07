using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Migrations
{
    public partial class nddfkdfkd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "DB_A57DC4_pnaDb_VendorAuth",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Paymentfrequency",
                table: "DB_A57DC4_pnaDb_Vendor",
                newName: "Vendor_code");

            migrationBuilder.RenameColumn(
                name: "Accounttype",
                table: "DB_A57DC4_pnaDb_Vendor",
                newName: "PhoneNumber");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TokenExpiry",
                table: "DB_A57DC4_pnaDb_VendorAuth",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddColumn<DateTime>(
                name: "Account_creation_date",
                table: "DB_A57DC4_pnaDb_Vendor",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Accountname",
                table: "DB_A57DC4_pnaDb_Vendor",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NightLifeOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vendor_code = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    OrderId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    item = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    item_price = table.Column<double>(nullable: false),
                    service_type = table.Column<string>(nullable: true),
                    demand_type = table.Column<string>(nullable: true),
                    MembershipID = table.Column<string>(nullable: true),
                    number_of_persons = table.Column<int>(nullable: false),
                    table_type = table.Column<string>(nullable: true),
                    reservation_date = table.Column<DateTime>(type: "Date", nullable: true),
                    service_category = table.Column<string>(nullable: true),
                    discounted_amount = table.Column<double>(nullable: false),
                    total_payable_amount = table.Column<double>(nullable: false),
                    IsVendorCredited = table.Column<bool>(nullable: true),
                    IsItem_paid = table.Column<bool>(nullable: true),
                    payment_status = table.Column<string>(nullable: true),
                    paymentMethod = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    paymentReference = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    payment_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NightLifeOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NightLifeProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vendor_code = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    item = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    item_price = table.Column<double>(nullable: false),
                    item_image = table.Column<string>(nullable: true),
                    date_created = table.Column<DateTime>(type: "Date", nullable: false),
                    item_code = table.Column<string>(nullable: true),
                    item_discount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NightLifeProducts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NightLifeOrder");

            migrationBuilder.DropTable(
                name: "NightLifeProducts");

            migrationBuilder.DropColumn(
                name: "Account_creation_date",
                table: "DB_A57DC4_pnaDb_Vendor");

            migrationBuilder.DropColumn(
                name: "Accountname",
                table: "DB_A57DC4_pnaDb_Vendor");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "DB_A57DC4_pnaDb_VendorAuth",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Vendor_code",
                table: "DB_A57DC4_pnaDb_Vendor",
                newName: "Paymentfrequency");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "DB_A57DC4_pnaDb_Vendor",
                newName: "Accounttype");

            migrationBuilder.AlterColumn<string>(
                name: "TokenExpiry",
                table: "DB_A57DC4_pnaDb_VendorAuth",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}