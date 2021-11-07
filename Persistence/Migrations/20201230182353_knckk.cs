using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Migrations
{
    public partial class knckk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipSubcription",
                table: "MembershipSubcription");

            migrationBuilder.RenameTable(
                name: "MembershipSubcription",
                newName: "MembershipSubscription");

            migrationBuilder.AddColumn<DateTime>(
                name: "membership_duedate",
                table: "MembershipSubscription",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipSubscription",
                table: "MembershipSubscription",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipSubscription",
                table: "MembershipSubscription");

            migrationBuilder.DropColumn(
                name: "membership_duedate",
                table: "MembershipSubscription");

            migrationBuilder.RenameTable(
                name: "MembershipSubscription",
                newName: "MembershipSubcription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipSubcription",
                table: "MembershipSubcription",
                column: "Id");
        }
    }
}