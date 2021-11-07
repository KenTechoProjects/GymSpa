namespace Domain.Notification.SMS
{
    public class Auth
    {
        public string username { get; set; }
        public string apikey { get; set; }
    }

    public class Message
    {
        public string sender { get; set; }
        public string messagetext { get; set; }
        public string flash { get; set; }
    }

    public class Gsm
    {
        public string msidn { get; set; }
        public string msgid { get; set; }
    }

    public class Recipients
    {
        public string gsm { get; set; }
    }

    public class SMS
    {
        public Auth auth { get; set; }
        public Message message { get; set; }
        public Recipients recipients { get; set; }
    }

    public class SmsReq
    {
        public SMS SMS { get; set; }
    }
}