﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

namespace Persistence.Migrations
{
    [DbContext(typeof(PNAContext))]
    [Migration("20210219181046_nddfkdfkd")]
    partial class nddfkdfkd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Persistence.Entities.FundWalletHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsWalletFunded")
                        .HasColumnType("bit");

                    b.Property<string>("WalletId")
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("amount")
                        .HasColumnType("float");

                    b.Property<string>("amountPaid")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("completed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("completedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("createdOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("currencyCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("customerEmail")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("customerName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("payableAmount")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("paymentDescription")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("paymentMethod")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("paymentReference")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("paymentStatus")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("transactionReference")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("FundWalletHistory");
                });

            modelBuilder.Entity("Persistence.Entities.MembershipSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsMembershipsubscribed")
                        .HasColumnType("bit");

                    b.Property<string>("MemberType")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("WalletId")
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("amount")
                        .HasColumnType("float");

                    b.Property<string>("amountPaid")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("completed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("completedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("createdOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("currencyCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("customerEmail")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("customerName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("membership_duedate")
                        .HasColumnType("datetime2");

                    b.Property<string>("payableAmount")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("paymentDescription")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("paymentMethod")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("paymentReference")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("paymentStatus")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("transactionReference")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("MembershipSubscription");
                });

            modelBuilder.Entity("Persistence.Entities.NightLifeProducts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Vendor_code")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("date_created")
                        .HasColumnType("Date");

                    b.Property<string>("item")
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("item_code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("item_discount")
                        .HasColumnType("float");

                    b.Property<string>("item_image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("item_price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("NightLifeProducts");
                });

            modelBuilder.Entity("Persistence.Entities.NightLife_Orders", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("IsItem_paid")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsVendorCredited")
                        .HasColumnType("bit");

                    b.Property<string>("MembershipID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Vendor_code")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("demand_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("discounted_amount")
                        .HasColumnType("float");

                    b.Property<string>("item")
                        .HasColumnType("nvarchar(150)");

                    b.Property<double>("item_price")
                        .HasColumnType("float");

                    b.Property<int>("number_of_persons")
                        .HasColumnType("int");

                    b.Property<string>("paymentMethod")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("paymentReference")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("payment_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("payment_status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("reservation_date")
                        .HasColumnType("Date");

                    b.Property<string>("service_category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("service_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("table_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("total_payable_amount")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("NightLifeOrder");
                });

            modelBuilder.Entity("Persistence.Entities.PnaAdmins", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateofBirth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HouseAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoftToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("TokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("autologin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("creationdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("fullname")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("last_activity")
                        .HasColumnType("datetime2");

                    b.Property<string>("last_login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("notes")
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("phonenumber")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("profile_pic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("user_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_Admins");
                });

            modelBuilder.Entity("Persistence.Entities.PnaMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActivationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CountryofResidence")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DateofBirth")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("IsActivated")
                        .HasColumnType("bit");

                    b.Property<string>("MemberTypeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MembershipID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumbers")
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Profession")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("QRcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferralCode")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TellUsAboutYourself")
                        .HasColumnType("nvarchar(Max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TransactionPin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TransactionPinSetupSatus")
                        .HasColumnType("bit");

                    b.Property<string>("WalletId")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WhyDoYouWantToJoin")
                        .HasColumnType("nvarchar(Max)");

                    b.Property<string>("subscriptionsStatus")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_Member");
                });

            modelBuilder.Entity("Persistence.Entities.PnaMemberAuth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastLoginDate")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MembershipID")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("PasswordChangeDate")
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(22)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TokenExpiry")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_MemberAuth");
                });

            modelBuilder.Entity("Persistence.Entities.PnaMemberType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ReferralLimit")
                        .HasColumnType("bigint");

                    b.Property<double>("SubscriptionAmount")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_MemberType");
                });

            modelBuilder.Entity("Persistence.Entities.PnaPaymentHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,3)");

                    b.Property<string>("Channel")
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("IsMerchantCredited")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MerchantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Narration")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TransferReference")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WalletId")
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_PaymentHistory");
                });

            modelBuilder.Entity("Persistence.Entities.PnaSuperAdmin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SoftToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("TokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("autologin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("creationdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("fullname")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("last_activity")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("last_login")
                        .HasColumnType("datetime2");

                    b.Property<string>("notes")
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("phonenumber")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("profile_pic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("roleid")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("user_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_SuperAdmin");
                });

            modelBuilder.Entity("Persistence.Entities.PnaSuperMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountCreateDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryofResidence")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DateofBirth")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MemberTypeId")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MembershipID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Profession")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("ReferralCode")
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("ReferralLimit")
                        .HasColumnType("bigint");

                    b.Property<string>("TellUsAboutYourself")
                        .HasColumnType("nvarchar(Max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TransactionPin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TransactionPinSetupSatus")
                        .HasColumnType("bit");

                    b.Property<string>("WalletId")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WhyDoYouWantToJoin")
                        .HasColumnType("nvarchar(Max)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_SuperMember");
                });

            modelBuilder.Entity("Persistence.Entities.PnaSuperMemberAuth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastLoginDate")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("PasswordChangeDate")
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(22)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TokenExpiry")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_SuperMemberAuth");
                });

            modelBuilder.Entity("Persistence.Entities.PnaVendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountManagerName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AccountManagerPhone")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Account_creation_date")
                        .HasColumnType("Date");

                    b.Property<string>("Accountname")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Accountnumber")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("BankName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Businesscategory")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Companyaddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Discountrate")
                        .HasColumnType("float");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Originalserviceamount")
                        .HasColumnType("float");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RCnumber")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TaxID")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Vendor_code")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("companyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_Vendor");
                });

            modelBuilder.Entity("Persistence.Entities.PnaVendorAuth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastLoginDate")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("PasswordChangeDate")
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("TokenExpiry")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_VendorAuth");
                });

            modelBuilder.Entity("Persistence.Entities.PnaWallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("WalletBalance")
                        .HasColumnType("float");

                    b.Property<string>("WalletId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_Wallet");
                });

            modelBuilder.Entity("Persistence.Entities.Sys_Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_Sys_Roles");
                });

            modelBuilder.Entity("Persistence.Entities.WalletTransferHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Narration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverFullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ReceiverPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ReceiverWalletId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SenderEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SenderFullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SenderPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SenderWalletId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TransactionType")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Transactionstatus")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("WalletTransferHistory");
                });

            modelBuilder.Entity("Persistence.Entities.sys_logs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Usertype")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("channel")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)");

                    b.Property<DateTime>("logDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DB_A57DC4_pnaDb_sys_logs");
                });
#pragma warning restore 612, 618
        }
    }
}
