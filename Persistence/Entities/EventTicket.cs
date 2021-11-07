using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistence.Entities
{
    [Table(nameof(EventTicket))]
 public   class EventTicket
    {
        public long Id { get; set; }
        public string Tickect { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? TicketDate { get; set; }
        //public int? EventId { get; set; }
        //[ForeignKey(nameof(EventId))]
        //public virtual Event Event { get; set; }
        public int? PnaMemberId { get; set; }
        [ForeignKey(nameof(PnaMemberId))]
        public virtual PnaMember PnaMember { get; set; }
        public int? VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendor Vendor { get; set; }

        public int? EventTicketManagerId { get; set; }
        [ForeignKey(nameof(EventTicketManagerId))]
        public virtual EventTicketManager EventTicketManager { get; set; }
    }
}
