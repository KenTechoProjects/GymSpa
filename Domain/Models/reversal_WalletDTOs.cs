namespace Domain.Models
{
    public class Data4
    {
        public int wallet_balance { get; set; }
        public string isTransaction_status { get; set; }
        public object walletId { get; set; }
    }

    public class reversal_WalletDTOs
    {
        public string requestId { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public Data4 data { get; set; }
    }
}