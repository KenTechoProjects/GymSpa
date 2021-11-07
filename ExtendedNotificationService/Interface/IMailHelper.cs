namespace ExtendedNotificationService.Interface
{
    public interface IMailHelper
    {
        void SendMail(string to, string subject, string content);
    }
}