using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    public partial class GymSpaSales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }

        public int? PnaVendorID { get; set; }

        [ForeignKey(nameof(PnaVendorID))]
        public virtual PnaVendor PnaVendor { get; set; }

        public DateTime? DateSoled { get; set; }
    }
}