using System;

namespace Domain.Models
{
    public class ResponseBodyd
    {
        public string paymentMethod { get; set; }
        public DateTime createdOn { get; set; }
        public double amount { get; set; }
        public double fee { get; set; }
        public string currencyCode { get; set; }
        public DateTime completedOn { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string paymentDescription { get; set; }
        public string paymentStatus { get; set; }
        public string transactionReference { get; set; }
        public string paymentReference { get; set; }
        public double payableAmount { get; set; }
        public double amountPaid { get; set; }
        public bool completed { get; set; }
    }

    public class GetqueryDTOs
    {
        public bool requestSuccessful { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        public ResponseBodyd responseBody { get; set; }
    }
}