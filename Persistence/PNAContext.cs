using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class PNAContext : DbContext
    {
        public PNAContext(DbContextOptions<PNAContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        public DbSet<sys_logs> sys_logs { get; set; }
        public DbSet<PnaSuperAdmin> PnaSuperAdmin { get; set; }
        public DbSet<PnaAdmins> PnaAdmins { get; set; }
        public DbSet<PnaMember> PnaMember { get; set; }
        public DbSet<PnaMemberAuth> PnaMemberAuth { get; set; }
        public DbSet<PnaVendor> PnaVendor { get; set; }
        public DbSet<PnaVendorAuth> PnaVendorAuth { get; set; }
        public DbSet<PnaWallet> PnaWallet { get; set; }
        public DbSet<PnaPaymentHistory> PnaPaymentHistory { get; set; }
        public DbSet<PnaMemberType> PnaMemberType { get; set; }
        public DbSet<Sys_Roles> Sys_Roles { get; set; }
        public DbSet<PnaSuperMember> PnaSuperMember { get; set; }
        public DbSet<PnaSuperMemberAuth> PnaSuperMemberAuth { get; set; }
        public DbSet<FundWalletHistory> FundWalletHistory { get; set; }
        public DbSet<MembershipSubscription> MembershipSubcription { get; set; }
        public DbSet<WalletTransferHistory> WalletTransferHistory { get; set; }
        public DbSet<NightLifeProducts> NightLifeProducts { get; set; }
        public DbSet<NightLifeOrders> NightLife_Orders { get; set; }
        public DbSet<night_life_order> night_life_order { get; set; }

        public DbSet<GymSpaSales> GymSpaSales { get; set; }
        public DbSet<Stock> GetGymSpaStocks { get; set; }
        public DbSet<GymSpaDates> GymSpaDates { get; set; }
        public DbSet<DiscountLevel> DiscountLevels { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<BaseService> BaseServices { get; set; }
        public DbSet<PnaVendorService> PnaVendorServices { get; set; }
        public DbSet<ErrorLogDb> ErrorLogDbs { get; set; }
        public DbSet<ErrorLogSolution> ErrorLogSolutions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Event>  Events { get; set; }
        public DbSet<EventTicket>  EventTickets { get; set; }
        public DbSet<EventTicketManager>  EventTicketManagers  { get; set; }
        public DbSet<EmergencyTicketManager>  EmergencyTicketManagers { get; set; }

    }
}
