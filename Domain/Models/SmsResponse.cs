namespace Domain.Models
{
    public class Response
    {
        public string status { get; set; }
        public int totalsent { get; set; }
        public int cost { get; set; }
    }

    public class SmsResponse
    {
        public Response response { get; set; }
    }
}