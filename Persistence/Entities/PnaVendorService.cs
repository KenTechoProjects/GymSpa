using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table(nameof(PnaVendorService))]
    /// <summary>
    /// Model that creates a one to many relationship between vendore and  services
    /// </summary>
    public class PnaVendorService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PnaVendorServiceId { get; set; }

        public int? VendoId { get; set; }
        public long? ServiceId { get; set; }

        [ForeignKey(nameof(VendoId))]
        public virtual PnaVendor PnaVendor { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public virtual BaseService Service { get; set; }
    }
}