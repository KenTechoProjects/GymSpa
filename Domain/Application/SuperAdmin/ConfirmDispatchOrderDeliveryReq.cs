using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperAdmin
{
    public class ConfirmDispatchOrderDeliveryReq
    {
        [Required]
        public string superAdminSoftToken { get; set; }

        public string superAdminUserID { get; set; }
        public int DispatchOrderRequestId { get; set; }
        public string DispatchCode { get; set; }
        public string BuyerorderReference { get; set; }
        public string Buyerorderproduct { get; set; }
    }
}