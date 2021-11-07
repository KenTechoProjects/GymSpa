using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Application.GymSpa.DTO
{
    public class ProductDTO
    {
        public ProductDTO()
        {
            ProductCategory = new ProductCategoryDTO();
            DiscountLevel = new DiscountLevelDTO();
            PnaVendors = new HashSet<PnaVendorDTO>();
        }

        public long Id { get; set; }

        public string Name { get; set; }
        public string Maker { get; set; }

        public string BarCordePath { get; set; }

        public DateTime ProductionYear { get; set; }

        public string Vendor_code { get; set; }

        public string Item { get; set; }
        public decimal Item_price { get; set; }
        public string Item_image { get; set; }

        public DateTime Date_Created { get; set; }
        public string Item_Code { get; set; }
        public int Item_Discount { get; set; }
        public long ProductCategoryId { get; set; }
        public long DiscountLevelId { get; set; }

        public virtual ProductCategoryDTO ProductCategory { get; set; }
        public virtual DiscountLevelDTO DiscountLevel { get; set; }
        public virtual ICollection<PnaVendorDTO> PnaVendors { get; set; }
        public virtual ICollection<OrderDetailDTO> OrderDetails { get; set; }
    }

    public class ProductDTOView
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Maker { get; set; }

        [Required]
        public string BarCordePath { get; set; }

        [Required]
        public DateTime ProductionYear { get; set; }

        [Required]
        public string Vendor_code { get; set; }

        [Required]
        public double Item_price { get; set; }

        [Required]
        public DateTime Date_Created { get; set; }

        [Required]
        public string Item_Code { get; set; }

        [Required]
        public int Item_Discount { get; set; }

        [Required]
        public long ProductCategoryId { get; set; }

        [Required]
        public long DiscountLevelId { get; set; }

        public IFormFile FileImage { get; set; }
        public UploadType UploadType { get; set; }
    }
}