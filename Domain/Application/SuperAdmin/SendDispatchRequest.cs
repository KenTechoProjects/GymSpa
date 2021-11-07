using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperAdmin
{
    public class SendDispatchRequest
    {
        [Required]
        public string superAdminSoftToken { get; set; }

        public string Buyerfirstname { get; set; }
        public string Buyerlastname { get; set; }
        public string Buyerstate_city { get; set; }
        public string Buyeraddress { get; set; }
        public string BuyerphoneNo { get; set; }
        public string Buyeremail { get; set; }
        public string BuyerorderReference { get; set; }
        public string Buyerorderproduct { get; set; }
        public string VendorCode { get; set; }
        public string Vendorfirstname { get; set; }
        public string Vendorlastname { get; set; }
        public string Vendorstate_city { get; set; }
        public string vendorShopAddress { get; set; }
        public string vendorShopName { get; set; }
        public string VendorphoneNo { get; set; }
        public string Vendoremail { get; set; }
        public string OrderRequestDate { get; set; }
        public string DispatchCode { get; set; }
        public string DispatchName { get; set; }
        public double BuyerorderproductPrice { get; set; }
    }
}