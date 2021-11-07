using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("DB_A57DC4_pnaDb_Stock")]
    public partial class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long TotalStock { get; set; }
        public long TotalSold { get; set; }
        public long TotalReTurned { get; set; }
        public long TotalSpoilt { get; set; }
        public virtual ICollection<Product> GymSpaProduct { get; set; }
        public int? PnaVendorID { get; set; }

        [ForeignKey(nameof(PnaVendorID))]
        public virtual PnaVendor PnaVendor { get; set; }
    }
}