namespace Domain.Models
{
    public class Debit_PNAreq
    {
        public double amount { get; set; }
        public string reference { get; set; }
        public string narration { get; set; }
        public string destinationBankCode { get; set; }
        public string destinationAccountNumber { get; set; }
    }
}