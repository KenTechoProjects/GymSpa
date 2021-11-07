using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class FundwalletDetails
    {
        [Column(TypeName = "nvarchar(50)")]
        public string paymentReference { get; set; }

        public bool IsWalletFunded { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string paymentMethod { get; set; }

        public Nullable<DateTime> createdOn { get; set; }
        public double amount { get; set; }
        public Nullable<DateTime> completedOn { get; set; }

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
    }
}