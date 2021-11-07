namespace Domain.Models
{
    public class Make_order_paymentReq
    {
        public string OrderId { get; set; }
        public string TransactionPin { get; set; }
        public string WalletId { get; set; }
        public double amount { get; set; }
    }
}