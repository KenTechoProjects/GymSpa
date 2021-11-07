using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistence.Migrations
{
    public partial class first00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_Admins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    fullname = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    phonenumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    user_type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    status = table.Column<string>(nullable: true),
                    last_login = table.Column<string>(nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    creationdate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    role = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    last_activity = table.Column<DateTime>(nullable: false),
                    autologin = table.Column<string>(nullable: true),
                    notes = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TokenExpiry = table.Column<DateTime>(nullable: false),
                    SoftToken = table.Column<string>(nullable: true),
                    profile_pic = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    HouseAddress = table.Column<string>(nullable: true),
                    DateofBirth = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_Member",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    DateofBirth = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ProfilePicture = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    CountryofResidence = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Profession = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TellUsAboutYourself = table.Column<string>(type: "nvarchar(Max)", nullable: true),
                    WhyDoYouWantToJoin = table.Column<string>(type: "nvarchar(Max)", nullable: true),
                    ReferralCode = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    WalletId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MemberTypeId = table.Column<string>(nullable: true),
                    QRcode = table.Column<string>(nullable: true),
                    subscriptionsStatus = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_Member", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_MemberAuth",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(22)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TokenExpiry = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LastLoginDate = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PasswordChangeDate = table.Column<string>(type: "nvarchar(60)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_MemberAuth", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_MemberType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true),
                    SubscriptionAmount = table.Column<double>(nullable: false),
                    ReferralLimit = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_MemberType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_PaymentHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Channel = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    WalletId = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    MerchantName = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    TransferReference = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IsMerchantCredited = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_PaymentHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_SuperAdmin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    fullname = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    phonenumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    user_type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    status = table.Column<string>(nullable: true),
                    last_login = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    creationdate = table.Column<DateTime>(nullable: false),
                    roleid = table.Column<int>(nullable: false),
                    role = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    last_activity = table.Column<DateTime>(nullable: false),
                    autologin = table.Column<string>(nullable: true),
                    notes = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TokenExpiry = table.Column<DateTime>(nullable: false),
                    SoftToken = table.Column<string>(nullable: true),
                    profile_pic = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_SuperAdmin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_SuperMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    DateofBirth = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ProfilePicture = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    CountryofResidence = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Profession = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TellUsAboutYourself = table.Column<string>(type: "nvarchar(Max)", nullable: true),
                    WhyDoYouWantToJoin = table.Column<string>(type: "nvarchar(Max)", nullable: true),
                    ReferralCode = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    WalletId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MemberTypeId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ReferralLimit = table.Column<long>(nullable: false),
                    AccountCreateDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_SuperMember", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_SuperMemberAuth",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(22)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TokenExpiry = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LastLoginDate = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PasswordChangeDate = table.Column<string>(type: "nvarchar(60)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_SuperMemberAuth", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_sys_logs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    logDate = table.Column<DateTime>(nullable: false),
                    Usertype = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    channel = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_sys_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_Sys_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_Sys_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_Vendor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Companyaddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Businesscategory = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Accountnumber = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Accounttype = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Originalserviceamount = table.Column<double>(nullable: false),
                    Discountrate = table.Column<double>(nullable: false),
                    AccountManagerName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    AccountManagerPhone = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TaxID = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    RCnumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Paymentfrequency = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_Vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_VendorAuth",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TokenExpiry = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LastLoginDate = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PasswordChangeDate = table.Column<string>(type: "nvarchar(60)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_VendorAuth", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DB_A57DC4_pnaDb_Wallet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    WalletBalance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DB_A57DC4_pnaDb_Wallet", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_Admins");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_Member");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_MemberAuth");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_MemberType");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_PaymentHistory");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_SuperAdmin");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_SuperMember");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_SuperMemberAuth");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_sys_logs");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_Sys_Roles");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_Vendor");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_VendorAuth");

            migrationBuilder.DropTable(
                name: "DB_A57DC4_pnaDb_Wallet");
        }
    }
}