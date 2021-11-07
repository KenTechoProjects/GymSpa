using System.Collections.Generic;

namespace Domain.Models
{
    public class ResponseBody
    {
        public string transactionReference { get; set; }
        public string paymentReference { get; set; }
        public string merchantName { get; set; }
        public string apiKey { get; set; }
        public string redirectUrl { get; set; }
        public IList<string> enabledPaymentMethod { get; set; }
        public string checkoutUrl { get; set; }
    }

    public class init_transactionDTO
    {
        public bool requestSuccessful { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        public ResponseBody responseBody { get; set; }
    }
}