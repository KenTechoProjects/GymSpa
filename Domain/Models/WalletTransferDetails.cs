using System;

namespace Domain.Models
{
    public class WalletTransferDetails
    {
        public string SenderFullName { get; set; }
        public string SenderWalletId { get; set; }
        public string SenderPhone { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverFullName { get; set; }
        public string ReceiverWalletId { get; set; }
        public string ReceiverPhone { get; set; }
        public double Amount { get; set; }
        public string Narration { get; set; }
        public string TransactionReference { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Transactionstatus { get; set; }
        public string TransactionType { get; set; }
    }
}