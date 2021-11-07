using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("WalletTransferHistory")]
    public class WalletTransferHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string SenderFullName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string SenderWalletId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string SenderPhone { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string SenderEmail { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string ReceiverFullName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string ReceiverWalletId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string ReceiverPhone { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public double Amount { get; set; }

        public string Narration { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string TransactionReference { get; set; }

        public DateTime TransactionDate { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Transactionstatus { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string TransactionType { get; set; }
    }
}