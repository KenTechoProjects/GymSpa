namespace Notification.Utilities.Binder
{
    public class NotificationConfig
    {
        public SMTPConfig SMTPConfig { get; set; }
        public SMSConfig SMSConfig { get; set; }
    }

    /**
 * Add below file to AppSettings.json
 * "NotificationConfig":{
 *      "SMTPConfig": {
 *          "Host": "10.10.14.41",
 *          "Port": 25,
 *          "Sender": "galaxy.services@fidelitybank.ng"
 *      },
 *      "SMSConfig": {
 *          "AccountNo": "",
 *          "APIUrl": "",
 *          isLive: false
 *      }
 * }
 * */
}