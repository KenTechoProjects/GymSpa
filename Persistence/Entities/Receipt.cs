using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table(nameof(Receipt))]
    public partial class Receipt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "money")]
        public decimal Total { get; set; }

        public DateTime TransactionDate { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string TransactionReference { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ServiceCategoty { get; set; }

        public long? OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public int? PnaMemberId { get; set; }

        [ForeignKey(nameof(PnaMemberId))]
        public virtual PnaMember PnaMember { get; set; }

        public int? VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendor Vendor { get; set; }
    }
}