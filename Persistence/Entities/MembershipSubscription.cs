using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("MembershipSubscription")]
    public class MembershipSubscription
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string WalletId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string paymentReference { get; set; }

        public bool IsMembershipsubscribed { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string MemberType { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string paymentMethod { get; set; }

        public Nullable<DateTime> createdOn { get; set; }
        public double amount { get; set; }
        public string currencyCode { get; set; }
        public Nullable<DateTime> completedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string customerName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string customerEmail { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string paymentDescription { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string paymentStatus { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string transactionReference { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public int payableAmount { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public double amountPaid { get; set; }

        public bool completed { get; set; }
        public Nullable<DateTime> membership_duedate { get; set; }
    }
}