namespace Domain.Models
{
    public class NightLife_OrdersDTO
    {
        public string OrderReference { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public string ItemDiscount { get; set; }
        public double Item_DiscountedPrice { get; set; }
        public double Total_payable { get; set; }
    }
}