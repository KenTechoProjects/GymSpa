using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table(nameof(ProductCategory))]
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        public int? VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendor PnaVendor { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}