using Domain.Application.GymSpa.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application.EventTickets.DTO
{
 public   class EmergencyTicketManagerDTO
    {

        public long Id { get; set; }

        public bool IsEmergency { get; set; }
        public string ManagerCode { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? VendorId { get; set; }
    
        public virtual PnaVendorDTO Vendor { get; set; }
        public long EventTicketManagerId { get; set; }
      
        public virtual EventTicketManagerDTO EventTicketManager { get; set; }
        public virtual ICollection<EventTicketDTO> EventTickets { get; set; }
        public virtual ICollection<EventDTO> Events { get; set; }

    }
}
