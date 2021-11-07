using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("NightLifeOrders")]
    public class night_life_order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Vendor_code { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string OrderId { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string item { get; set; }

        public double item_price { get; set; }
        public string service_type { get; set; }
        public string demand_type { get; set; }
        public string MembershipID { get; set; }
        public int number_of_persons { get; set; }
        public string table_type { get; set; }

        [Column(TypeName = "Date")]
        public Nullable<DateTime> reservation_date { get; set; }

        public string service_category { get; set; }
        public double discounted_amount { get; set; }
        public double total_payable_amount { get; set; }
        public Nullable<bool> IsVendorCredited { get; set; }
        public Nullable<bool> IsItem_paid { get; set; }
        public string payment_status { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string paymentMethod { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string paymentReference { get; set; }

        public Nullable<DateTime> payment_date { get; set; }

        [Column(TypeName = "Date")]
        public DateTime order_date { get; set; }
    }
}