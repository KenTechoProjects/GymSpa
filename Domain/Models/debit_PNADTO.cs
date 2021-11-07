namespace Domain.Models
{
    public class ResponseBody2
    {
        public double amount { get; set; }
        public string reference { get; set; }
        public string status { get; set; }
        public string dateCreated { get; set; }
        public double totalFee { get; set; }
    }

    public class debit_PNADTO
    {
        public bool requestSuccessful { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        public ResponseBody2 responseBody { get; set; }
    }
}