using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("DB_A57DC4_pnaDb_Order")]
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Vendor_code { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public int Item_Discount { get; set; }

        public string OrderCode { get; set; }
        public string OrderId { get; set; }
        public string Item { get; set; }
        public decimal Discounted_amount { get; set; }
        public string ItemCode { get; set; }
        public DateTime Order_date { get; set; }
        public int? VendorID { get; set; }

        [ForeignKey(nameof(VendorID))]
        public virtual PnaVendor PnaVendor { get; set; }

        public double TotalCost { get; set; }
        public int ProductCount { get; set; }
        public double Discount { get; set; }
        public double NetCost { get; set; }

        public string MembershipID { get; set; }
        public string Service_type { get; set; }
        public string Demand_Type { get; set; }
        public string Service_category { get; set; }
        public decimal Total_payable_amount { get; set; }
        public Nullable<bool> IsVendorCredited { get; set; }
        public Nullable<bool> IsItem_paid { get; set; }
        public string Payment_status { get; set; }
        public string PaymentMethod { get; set; }
        public string Table_type { get; set; }
        public decimal Item_price { get; set; }
        public string PaymentReference { get; set; }
        public int Number_of_persons { get; set; }
        public int Product { get; set; }
        public Nullable<DateTime> Payment_date { get; set; }
        public Nullable<DateTime> Reservation_date { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<GymSpaSales> GymSpaSales { get; set; }
    }
}