using Domain.Application.GymSpa.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.EventTickets.DTO
{

    public class EventTicketDTO
    {
        public long Id { get; set; }
        public string Tickect { get; set; }
        [DataType(DataType.Date)]
        public DateTime TicketDate { get; set; }
        //public int? EventId { get; set; }
        //[ForeignKey(nameof(EventId))]
        //public virtual EventDTO Event { get; set; }
        public int PnaMemberId { get; set; }

        public virtual PnaMemberDTO PnaMember { get; set; }
        public int VendorId { get; set; }

        public virtual PnaVendorDTO Vendor { get; set; }

        public int EventTicketManagerId { get; set; }

        public virtual EventTicketManagerDTO EventTicketManager { get; set; }
    }
}
