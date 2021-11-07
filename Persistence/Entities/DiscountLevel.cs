using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table(nameof(DiscountLevel))]
    public partial class DiscountLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public double Discount { get; set; }
        public int Discount_Level { get; set; }
        public long? ProductID { get; set; }
        public int? PnaVendorID { get; set; }
        public virtual PnaVendor PnaVendor { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}