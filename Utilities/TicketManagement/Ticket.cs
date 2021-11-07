using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using Microsoft.Extensions.Options;
using Domain.Application.EventTickets.DTO;

namespace Utilities.TicketManagement
{
 public   class Ticket
    {
        private readonly Logger _logger;
        private TicketSettings _ticketSettings;
        public Ticket(Utilities.Logger logger,IOptions<TicketSettings> ticketSettings)
        {
            _logger = logger;
            _ticketSettings=ticketSettings.Value;;
        }

        public string TicketNumber(int totalWidth)
        {

            string data = string.Empty;
            string ticket=data.PadLeft()
        }
        public string UploadWebCamImage(string imageData,string imagePath)
        {
            string filename = Server.MapPath(imagePath) + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "") + ".png";
           // string filename = Server.MapPath("~/UploadWebcamImages/webcam_") + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "") + ".png";
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }
            return "success";
        }

    }
}
