using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    public class DutyRoaster
    {
        public long Id { get; set; }
        public string StaffId { get; set; }
        public virtual Staff Staff { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendor PnaVendor { get; set; }

        public int? RequestorId { get; set; }

        [ForeignKey(nameof(RequestorId))]
        public virtual PnaVendor PnaVendorRequestor { get; set; }
    }
}