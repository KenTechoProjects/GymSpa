using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Application.GymSpa.DTO
{
    public class StockDTO
    {
        public long Id { get; set; }
        public long TotalStock { get; set; }
        public long TotalSold { get; set; }
        public long TotalReTurned { get; set; }
        public long TotalSpoilt { get; set; }
        public virtual ICollection<ProductDTO> GymSpaProduct { get; set; }
        public int PnaVendorID { get; set; }
        public virtual PnaVendorDTO PnaVendor { get; set; }
    }

    public class StockDTOView
    {
        [Required]
        public long TotalStock { get; set; }

        [Required]
        public long TotalSold { get; set; }

        [Required]
        public long TotalReTurned { get; set; }

        [Required]
        public long TotalSpoilt { get; set; }

        [Required]
        public int PnaVendorID { get; set; }
    }
}