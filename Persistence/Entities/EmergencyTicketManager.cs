using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistence.Entities
{
    [Table(nameof(EmergencyTicketManager))]
    public class EmergencyTicketManager
    {
        public long Id { get; set; }

        public bool IsEmergency { get; set; }
        public string ManagerCode { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendor Vendor { get; set; }
        public long? EventTicketManagerId { get; set; }
        [ForeignKey(nameof(EventTicketManagerId))]
        public virtual EventTicketManager EventTicketManager{get;set;}
        public virtual ICollection<EventTicket> EventTickets { get; set; }
        public virtual ICollection<Event> Events { get; set; }

    }
}
