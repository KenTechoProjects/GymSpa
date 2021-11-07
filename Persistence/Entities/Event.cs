using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistence.Entities
{
    [Table(nameof(Event))]
  public  class Event
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ItemCode { get; set; }
  [Column(TypeName = "Time")]
        public DateTime? TimeOfEvent { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOfEvent { get; set; }
        public int? VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendor Vendor { get; set; }
        public long NumberOfPerson { get; set; }
        public decimal Amount { get; set; }
        public int Discount { get; set; }
        // public virtual ICollection<EventTicketManager> EventTicketManagers { get; set; }

        public int? EventTicketManagerId { get; set; }
        [ForeignKey(nameof(EventTicketManagerId))]
        public virtual EventTicketManager EventTicketManager { get; set; }
    }
}
