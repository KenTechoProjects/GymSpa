using Domain.DTO.Notification.Email;
using System.Threading.Tasks;

namespace Notification.Interface
{
    public interface INotificationServices
    {
        //Task<ResponseParam> SendSMS(string Phone, string Message);
        //Task<ResponseParam> SendSMS2(SmsReq smsReq);
        // string CreateEmailBody(string templatePath);
        // SendEmailDTO SendEmail(SendEmailReq emailReq);
        string SendEmail(string MailTo, string htmlBody, string MailSubject);

        Task<string> sendmail(SendEmailApiReq sendEmailApiReq);
    }
}