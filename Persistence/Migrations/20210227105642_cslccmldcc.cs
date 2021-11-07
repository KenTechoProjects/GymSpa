using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Migrations
{
    public partial class cslccmldcc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NightLifeOrders",
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
                    payment_date = table.Column<DateTime>(nullable: true),
                    order_date = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NightLifeOrders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NightLifeOrders");
        }
    }
}