using System;

namespace Domain.DTO.Notification.Email
{
    public class SendEmailApiReq
    {
        public string MailTo { get; set; }
        public Object htmlBody { get; set; }
        public string MailSubject { get; set; }
    }
}