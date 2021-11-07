using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("NightLifeProducts")]
    public class NightLifeProducts
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Vendor_code { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string item { get; set; }

        public double item_price { get; set; }
        public string item_image { get; set; }

        [Column(TypeName = "Date")]
        public DateTime date_created { get; set; }

        public string item_code { get; set; }
        public int item_discount { get; set; }
    }
}