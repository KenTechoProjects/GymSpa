using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MembershipID",
                table: "DB_A57DC4_pnaDb_SuperMember",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionPin",
                table: "DB_A57DC4_pnaDb_SuperMember",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TransactionPinSetupSatus",
                table: "DB_A57DC4_pnaDb_SuperMember",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MembershipID",
                table: "DB_A57DC4_pnaDb_Member",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionPin",
                table: "DB_A57DC4_pnaDb_Member",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TransactionPinSetupSatus",
                table: "DB_A57DC4_pnaDb_Member",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipID",
                table: "DB_A57DC4_pnaDb_SuperMember");

            migrationBuilder.DropColumn(
                name: "TransactionPin",
                table: "DB_A57DC4_pnaDb_SuperMember");

            migrationBuilder.DropColumn(
                name: "TransactionPinSetupSatus",
                table: "DB_A57DC4_pnaDb_SuperMember");

            migrationBuilder.DropColumn(
                name: "MembershipID",
                table: "DB_A57DC4_pnaDb_Member");

            migrationBuilder.DropColumn(
                name: "TransactionPin",
                table: "DB_A57DC4_pnaDb_Member");

            migrationBuilder.DropColumn(
                name: "TransactionPinSetupSatus",
                table: "DB_A57DC4_pnaDb_Member");
        }
    }
}