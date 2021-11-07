using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Application.GymSpa.DTO
{
    public class ProductCategoryDTO
    {
        public ProductCategoryDTO()
        {
            Products = new HashSet<ProductDTO>();
        }
        public long Id { get; set; }

        public string Name { get; set; }
        public int VendorId { get; set; }
        
        public virtual PnaVendorDTO PnaVendor { get; set; }
       
        public virtual ICollection<ProductDTO> Products { get; set; }
    }
    [AutoMap(typeof(ProductCategoryDTO))] 
    public class ProductCategoryDTOView
    {
        
        

        public string Name { get; set; }
        public int VendorId { get; set; }
       }
    }

