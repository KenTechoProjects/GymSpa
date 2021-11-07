using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperAdmin
{
    public class ConfirmVendorOrderDeliveryReq
    {
        [Required]
        public string superAdminSoftToken { get; set; }

        public string superAdminUserID { get; set; }
        public string Vendorfirstname { get; set; }
        public double Buyerorderproductprice { get; set; }
        public string BuyerorderReference { get; set; }
        public string Buyerorderproduct { get; set; }
        public string DispatchCompany { get; set; }
        public string Delivery_status { get; set; }
        public string DeliverDate { get; set; }
        public string AssigntoRider { get; set; }
    }
}