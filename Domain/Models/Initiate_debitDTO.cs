namespace Domain.Models
{
    public class Initiate_debitDTO
    {
        public double amount { get; set; }
        public string reference { get; set; }
        public string narration { get; set; }
        public string destinationBankCode { get; set; }
        public string destinationAccountNumber { get; set; }
        public string currency { get; set; }
        public string sourceAccountNumber { get; set; }
    }
}