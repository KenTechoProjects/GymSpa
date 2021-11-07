using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table(nameof(OrderDetail))]
    public partial class OrderDetail
    {
        [Column(TypeName = "bigint")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string Item { get; set; }

        public double Item_price { get; set; }

        [Column(TypeName = "Date")]
        public Nullable<DateTime> Order_date { get; set; }

        public long? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public double Discounted_amount { get; set; }
        public double DiscountedInPercent { get; set; }
        public string MembershipId { get; set; }
        public int? VendorId { get; set; }
        public int Qunatity { get; set; }
        //public double Discounted_amount { get; set; }
        //public double Total_payable_amount { get; set; }
        //public Nullable<bool> IsVendorCredited { get; set; }
        //public Nullable<bool> IsItem_paid { get; set; }
        //public string Payment_status { get; set; }
        //[Column(TypeName = "nvarchar(50)")]
        //public string PaymentMethod { get; set; }
        //[Column(TypeName = "nvarchar(50)")]
        //public string PaymentReference { get; set; }
        //public Nullable<DateTime> Payment_date { get; set; }

        public long? OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }
    }
}