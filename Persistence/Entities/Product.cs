using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table(nameof(Product))]
    public class Product
    {
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        public string Maker { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string BarCordePath { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? ProductionYear { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Vendor_code { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public decimal Item_price { get; set; }

        public string Item { get; set; }
        public string Item_image { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date_Created { get; set; }

        public string Item_Code { get; set; }
        public decimal Item_Discount { get; set; }
        public long? ProductCategoryId { get; set; }

        [ForeignKey(nameof(ProductCategoryId))]
        public virtual ProductCategory ProductCategory { get; set; }

        public long? DiscountLevelId { get; set; }

        [ForeignKey(nameof(DiscountLevelId))]
        // public virtual DiscountLevel DiscountLevel { get; set; }
        //public virtual ICollection<PnaVendor> PnaVendors { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}