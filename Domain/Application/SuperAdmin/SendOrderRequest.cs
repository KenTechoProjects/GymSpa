using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperAdmin
{
    public class SendOrderRequest
    {
        [Required]
        public string superAdminSoftToken { get; set; }

        [Required]
        public string Buyerfirstname { get; set; }

        [Required]
        public string Buyerlastname { get; set; }

        [Required]
        public string Buyerstate_city { get; set; }

        [Required]
        public string Buyeraddress { get; set; }

        [Required]
        public string BuyerphoneNo { get; set; }

        [Required]
        public string Buyeremail { get; set; }

        [Required]
        public string BuyerorderReference { get; set; }

        [Required]
        public string Buyerorderproduct { get; set; }

        [Required]
        public string VendorCode { get; set; }

        public string vendorfirsrname { get; set; }
        public string vendorlastname { get; set; }
        public string vendorphone { get; set; }
        public string vendorShopname { get; set; }
        public string vendorShopAddress { get; set; }
        public string vendorstate_city { get; set; }
        public string Vendoremail { get; set; }
        public double BuyerorderproductPrice { get; set; }
    }
}