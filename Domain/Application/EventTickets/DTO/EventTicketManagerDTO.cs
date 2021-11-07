using Domain.Application.GymSpa.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.EventTickets.DTO
{
    [Table(nameof(EventTicketManagerDTO))]
 public   class EventTicketManagerDTO
    {

        public long Id { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string ManagerCode { get; set; }


        [Column(TypeName = "nvarchar(15)")]
        public long? StaffId { get; set; }
        [ForeignKey(nameof(StaffId))]
        public virtual StaffDTO Staff { get; set; }
        public virtual ICollection<EventTicketDTO> EventTickets { get; set; }
        public virtual ICollection<EventDTO> Events { get; set; }
        public virtual ICollection<EmergencyTicketManagerDTO> EmergencyTicketManagers { get; set; }
        public int? VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendorDTO Vendor { get; set; }

    }
}
