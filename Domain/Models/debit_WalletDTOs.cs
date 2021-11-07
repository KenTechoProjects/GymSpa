namespace Domain.Models
{
    public class Data
    {
        public string IsTransaction_status { get; set; }
        public double Wallet_balance { get; set; }
        public string WalletId { get; set; }
    }

    public class debit_WalletDTOs
    {
        public string RequestId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
        //public IList<Data> Data { get; set; }
    }
}