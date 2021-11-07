using System;

namespace Domain.Notification
{
    public class SendEmailApi
    {
        public string MailTo { get; set; }
        public Object htmlBody { get; set; }
        public string MailSubject { get; set; }
    }
}