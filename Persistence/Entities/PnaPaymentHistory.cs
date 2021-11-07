using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("DB_A57DC4_pnaDb_PaymentHistory")]
    public class PnaPaymentHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string Channel { get; set; } //Name of client app sending payment request

        [Column(TypeName = "nvarchar(150)")]
        public string WalletId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Narration { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public double Amount { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(80)")]
        public string MerchantName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string TransferReference { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string TransactionStatus { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string IsMerchantCredited { get; set; }
    }
}