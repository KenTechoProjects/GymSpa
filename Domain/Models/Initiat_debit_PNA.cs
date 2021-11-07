using System;

namespace Domain.Models
{
    public class ResponseBody3
    {
        public double amount { get; set; }
        public string reference { get; set; }
        public string status { get; set; }
        public DateTime dateCreated { get; set; }
        public double totalFee { get; set; }
    }

    public class Data3
    {
        public bool requestSuccessful { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        public ResponseBody3 responseBody { get; set; }
    }

    public class Initiat_debit_PNA
    {
        public string requestId { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public Data3 data { get; set; }
    }
}