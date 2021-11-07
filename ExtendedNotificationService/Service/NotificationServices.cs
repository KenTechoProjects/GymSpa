using Domain.DTO.Notification.Email;
using Microsoft.Extensions.Options;
using Notification.Interface;
using Notification.Utilities.Binder;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;
using Utilities.Interface;
using Utilities.SystemActivity.Interface;

namespace Notification.Service
{
    public class NotificationServices : INotificationServices
    {
        private readonly BaseUrls _baseUrls;
        private readonly UIHttpClient _httpClient;
        private readonly Logger _logger;
        private readonly NotificationConfig _notificationConfig;
        private readonly ISystemActivityService _systemActivityService;

        public NotificationServices(IOptions<BaseUrls> baseUrls, UIHttpClient httpClient, IOptions<NotificationConfig> config, Logger logger, ISystemActivityService systemUtility)
        {
            _httpClient = httpClient;
            _baseUrls = baseUrls.Value;
            _logger = logger;
            _notificationConfig = config.Value;
            _systemActivityService = systemUtility;
        }

        public string SendEmail(string MailTo, string htmlBody, string MailSubject)
        {
            try
            {
                MailAddress to = new MailAddress(MailTo);
                MailAddress from = new MailAddress(_notificationConfig.SMTPConfig.Sender);
                MailMessage mail = new MailMessage(from, to);
                mail.IsBodyHtml = true;
                mail.Subject = MailSubject;
                mail.Body = htmlBody;
                //SmtpClient smtp = new SmtpClient(_notificationConfig.SMTPConfig.Host, _notificationConfig.SMTPConfig.Port);

                //if (!string.IsNullOrWhiteSpace(_notificationConfig.SMTPConfig.Password))
                //{
                //    smtp.UseDefaultCredentials = true;
                //    smtp.Credentials = new NetworkCredential(_notificationConfig.SMTPConfig.Sender,_notificationConfig.SMTPConfig.Password);
                //    smtp.EnableSsl = true;
                //}
                //using (SmtpClient smtp = new SmtpClient(_notificationConfig.SMTPConfig.Host, _notificationConfig.SMTPConfig.Port))
                //{
                //    smtp.UseDefaultCredentials = false;
                //    smtp.Credentials = new NetworkCredential(_notificationConfig.SMTPConfig.Sender, _notificationConfig.SMTPConfig.Password);
                //    smtp.EnableSsl = true;
                //    smtp.Send(mail);
                //    return "Ok";
                //}

                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    client.Credentials = new NetworkCredential(_notificationConfig.SMTPConfig.Sender, _notificationConfig.SMTPConfig.Password);
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Send(mail);
                    return "Ok";
                }
            }
            catch (Exception ex)
            {
                return $"[error] => {ex.Message}";
            }
        }

        public async Task<string> sendmail(SendEmailApiReq sendEmailApiReq)
        {
            MailAddress to = new MailAddress(sendEmailApiReq.MailTo);
            MailAddress from = new MailAddress(_notificationConfig.SMTPConfig.Sender, "PNA Service");
            MailMessage mail = new MailMessage(from, to);
            mail.IsBodyHtml = true;
            mail.Subject = sendEmailApiReq.MailSubject;
            mail.Body = sendEmailApiReq.htmlBody.ToString();
            // string str1 = "gmail.com";
            string str2 = sendEmailApiReq.MailTo;
            //SmtpClient smtp = new SmtpClient("mail.mainmart.com.ng", 25);
            SmtpClient sc = new SmtpClient();
            sc.Host = _notificationConfig.SMTPConfig.Host;
            try
            {
                sc.Port = _notificationConfig.SMTPConfig.Port;
                sc.Credentials = new System.Net.NetworkCredential(_notificationConfig.SMTPConfig.Sender, _notificationConfig.SMTPConfig.Password);
                sc.EnableSsl = false;
                sc.Send(mail);
                return "Ok";
            }
            catch (Exception ex)
            {
                return $"[error] => {ex.Message}";
                await _systemActivityService.JsonErrorLog($"[VeexcelAPI][Send email][Response] => {ex.Message}");
            }
        }
    }
}