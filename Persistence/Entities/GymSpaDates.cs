using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table(nameof(GymSpaDates))]
    public partial class GymSpaDates
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime? Availabledate { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsClosed { get; set; }
        public bool IsActive { get; set; }
        public string VendorCode { get; set; }
        public int? PnaVendorID { get; set; }

        [ForeignKey(nameof(PnaVendorID))]
        public virtual PnaVendor PnaVendor { get; set; }
    }
}