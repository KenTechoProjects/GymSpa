using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistence.Entities
{
    [Table(nameof(EventTicketManager))]
 public   class EventTicketManager
    {
        public long Id { get; set; }
        [Column(TypeName ="nvarchar(10)")]
        public string ManagerCode { get; set; }

      
        [Column(TypeName = "nvarchar(15)")]
        public long? StaffId { get; set; } 
        [ForeignKey(nameof(StaffId))]
        public virtual Staff Staff { get; set; }
        public virtual ICollection<EventTicket> EventTickets { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<EmergencyTicketManager>  EmergencyTicketManagers { get; set; }
        public int? VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendor Vendor { get; set; }

        
    }
}
