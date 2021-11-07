using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Application.GymSpa.DTO
{
    public class PrintOrder
    {
        [Required(ErrorMessage = "Vendor Code is required")]
        [DisplayName("Vendor Code")]
        public string Vendor_code { get; set; }

        [Required(ErrorMessage = "Order Id is required")]
        [DisplayName("Order")]
        public string OrderId { get; set; }

        [Required(ErrorMessage = "Service type is required")]
        [DisplayName("Service Type")]
        public string Service_type { get; set; }

        [Required(ErrorMessage = "Payment date is required")]
        [DisplayName("Payment Date")]
        public DateTime payment_date { get; set; }
    }

    public class OrderDTO
    {
        public decimal Item_price { get; set; }
        public long Id { get; set; }
        public string Item_Discount { get; set; }
        public string Table_type { get; set; }
        public string Vendor_code { get; set; }
        public string OrderCode { get; set; }
        public string ItemCode { get; set; }
        public int Number_of_persons { get; set; }

        public DateTime Order_date { get; set; }

        [Required]
        [DisplayName("Vendor ID")]
        public int? VendorID { get; set; }

        public Nullable<DateTime> Reservation_date { get; set; }
        public virtual PnaVendorDTO PnaVendor { get; set; }
        public double TotalCost { get; set; }
        public string Item { get; set; }
        public int ProductCount { get; set; }
        public double Discount { get; set; }
        public double NetCost { get; set; }

        [Required]
        public string MembershipID { get; set; }

        [Required]
        public string Service_type { get; set; }

        [Required]
        public string Demand_Type { get; set; }

        [Required]
        public string Service_category { get; set; }

        public double Total_payable_amount { get; set; }
        public Nullable<bool> IsVendorCredited { get; set; }
        public Nullable<bool> IsItem_paid { get; set; }
        public string Payment_status { get; set; }
        public string PaymentMethod { get; set; }
        public int Product { get; set; }
        public string PaymentReference { get; set; }
        public Nullable<DateTime> Payment_date { get; set; }
        public bool Deleted { get; set; }
        public virtual ICollection<OrderDetailDTO> OrderDetails { get; set; }
    }

    public class OrderDTORequest
    {
        [Required]
        public string Vendor_code { get; set; }

        [Required]
        public string ItemCode { get; set; }

        [Required]
        public string Service_type { get; set; }

        [Required]
        public string Demand_Type { get; set; }

        [Required]
        public string MembershipID { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public int Number_of_persons { get; set; }
        public string Table_type { get; set; }
        public Nullable<DateTime> Reservation_date { get; set; }

        public string Service_category { get; set; }
    }

    public class OrderDTORequests
    {
        [Required]
        public string Vendor_code { get; set; }

        [Required]
        public string ItemCode { get; set; }

        [Required]
        public string Service_type { get; set; }

        [Required]
        public string Demand_Type { get; set; }

        [Required]
        public string MembershipID { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public string Table_type { get; set; }

        public string Service_category { get; set; }
    }

    public class PrintReceiptRequest
    {
        [Required]
        public int vendorId { get; set; }

        [Required]
        public string MembershipId { get; set; }

        public long OrderId { get; set; }
    }

    public class Order_View
    {
        public bool Deleted { get; set; }
        public string Item { get; set; }
        public decimal Item_price { get; set; }
        public decimal Total_payable { get; set; }
        public decimal Item_DiscountedPrice { get; set; }
        public string OrderReference { get; set; }
        public string Item_Discount { get; set; }
    }

    public class OrderView
    {
        public bool Deleted { get; set; }
        public string Id { get; set; }

        public string Vendor_code { get; set; }

        public string OrderCode { get; set; }

        public DateTime Order_date { get; set; }

        [Required]
        [DisplayName("Vendor ID")]
        public int? VendorID { get; set; }

        public virtual PnaVendorDTO PnaVendor { get; set; }
        public double TotalCost { get; set; }
        public int ProductCount { get; set; }

        [Required]
        [DisplayName("Discount In %")]
        public double Discount { get; set; }

        public double NetCost { get; set; }

        [Required]
        public string MembershipID { get; set; }

        [Required]
        public string Service_type { get; set; }

        [Required]
        public string Demand_Type { get; set; }

        [Required]
        public string Service_category { get; set; }

        public double Total_payable_amount { get; set; }
        public Nullable<bool> IsVendorCredited { get; set; }
        public Nullable<bool> IsItem_paid { get; set; }
        public string Payment_status { get; set; }
        public string PaymentMethod { get; set; }

        public string PaymentReference { get; set; }
        public Nullable<DateTime> Payment_date { get; set; }
    }

    public class OrderPersonalDTO
    {
        public string Id { get; set; }

        public string Vendor_code { get; set; }

        public string OrderId { get; set; }

        public DateTime Order_date { get; set; }
        public int VendorID { get; set; }

        public virtual PnaVendorDTO PnaVendor { get; set; }
        public double TotalCost { get; set; }
        public int ProductCount { get; set; }
        public string memberId { get; set; }
        public double Discount { get; set; }
        public double NetCost { get; set; }

        public string MembershipID { get; set; }
        public string Service_type { get; set; }
        public string Demand_Type { get; set; }
        public string Service_category { get; set; }
        public double Total_payable_amount { get; set; }

        public Nullable<bool> IsVendorCredited { get; set; }
        public Nullable<bool> IsItem_paid { get; set; }
        public string Payment_status { get; set; }
        public string PaymentMethod { get; set; }

        public string PaymentReference { get; set; }
        public Nullable<DateTime> Payment_date { get; set; }
        public virtual ICollection<OrderDetailDTO> OrderDetails { get; set; }
    }

    public class OrderView_
    {
        [Required]
        public string Vendor_code { get; set; }

        [Required]
        public string OrderId { get; set; }

        [Required]
        public string Item { get; set; }

        [Required]
        public double Item_price { get; set; }

        [Required]
        public string Service_type { get; set; }

        [Required]
        public string Demand_type { get; set; }

        [Required]
        public string MembershipID { get; set; }

        [Required]
        public string Discounted_amount { get; set; }

        [Required]
        public string Service_category { get; set; }

        [Required]
        public double Discount { get; set; }

        [Required]
        public double Total_payable_amount { get; set; }

        [Required]
        public bool IsVendorCredited { get; set; }

        public bool IsItem_paid { get; set; }

        [Required]
        public PaymentStatus Payment_status { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public string PaymentReference { get; set; }

        [Required]
        public string OrderReference { get; set; }

        [Required]
        public DateTime payment_date { get; set; }

        [Required]
        public DateTime? Order_date { get; set; }

        [Required]
        public long? VendorID { get; set; }
    }
}