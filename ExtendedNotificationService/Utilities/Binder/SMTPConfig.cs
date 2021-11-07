namespace Notification.Utilities.Binder
{
    /**
       * Mail Configuration
       * Sender: galaxy.services@fidelitybank.ng
       * Host: 10.10.14.41
       * Port: 25
       * **/

    public class SMTPConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
    }
}