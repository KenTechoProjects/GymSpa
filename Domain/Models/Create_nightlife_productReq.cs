using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Create_nightlife_productReq
    {
        public string vendorcode { get; set; }
        public string item { get; set; }

        [Required]
        public double item_price { get; set; }

        [Required]
        public IFormFile item_image { get; set; }

        [Required]
        public int item_discount { get; set; }
    }
}