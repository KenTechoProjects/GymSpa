using Domain.Application.GymSpa.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.EventTickets.DTO
{
     
  public  class EventDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ItemCode { get; set; }
        [DataType(DataType.Date)]
        public DateTime TimeOfEvent { get; set; }
  [DataType(DataType.Date)]
        public DateTime? DateOfEvent { get; set; }
        public int VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendorDTO Vendor { get; set; }
        public long NumberOfPerson { get; set; }
        public decimal Amount { get; set; }
        public int Discount { get; set; }
        // public virtual ICollection<EventTicketManager> EventTicketManagers { get; set; }

        public int EventTicketManagerId { get; set; }
        [ForeignKey(nameof(EventTicketManagerId))]
        public virtual EventTicketManagerDTO EventTicketManager { get; set; }


    }
}
