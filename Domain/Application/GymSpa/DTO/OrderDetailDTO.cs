using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Application.GymSpa.DTO
{
    public class OrderDetailDTO
    {
        public long Id { get; set; }

        public string Item { get; set; }
        public double Item_price { get; set; }

        public double Discounted_amount { get; set; }
        public double DiscountedInPercent { get; set; }

        [DataType(DataType.Date)]
        public DateTime Order_date { get; set; }

        public long OrderId { get; set; }
        public virtual OrderDTO Order { get; set; }

        public long ProductId { get; set; }

        public string MembershipId { get; set; }
        public virtual ProductDTO Product { get; set; }
        public int Qunatity { get; set; }
    }

    public class OrderDetailDTOView
    {
        [Required]
        public string Item { get; set; }

        [Required] public double Item_price { get; set; }

        public double Discounted_amount { get; set; }

        [Required]
        public DateTime Order_date { get; set; }

        [Required]
        public long OrderId { get; set; }

        public double DiscountedInPercent { get; set; }

        [Required]
        public long ProductId { get; set; }

        [Required]
        public string MembershipId { get; set; }

        [Required]
        public virtual OrderDTO Order { get; set; }

        [Required]
        public int Qunatity { get; set; }
    }

    public class OrderDetailDTO_
    {
        public long Id { get; set; }
        public string Item { get; set; }
        public double Item_price { get; set; }
        public string Service_type { get; set; }
        public string Demand_Type { get; set; }
        public string MembershipID { get; set; }

        public string Table_type { get; set; }

        public Nullable<DateTime> Reservation_date { get; set; }
        public string Service_category { get; set; }
        public int ProductCount { get; set; }
        public double Discounted_amount { get; set; }
        public double Total_payable_amount { get; set; }
        public Nullable<bool> IsVendorCredited { get; set; }
        public Nullable<bool> IsItem_paid { get; set; }
        public string Payment_status { get; set; }
        public Nullable<DateTime> Order_date { get; set; }
        public string PaymentMethod { get; set; }
        public int Qunatity { get; set; }
        public string PaymentReference { get; set; }
        public Nullable<DateTime> Payment_date { get; set; }

        public long? OrderId { get; set; }
        public string OrderId_ { get; set; }
        public virtual OrderDTO Order { get; set; }
    }
}