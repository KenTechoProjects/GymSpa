using System.Collections.Generic;

namespace Domain.Application.GymSpa.DTO
{
    public class DiscountLevelDTO
    {
        public DiscountLevelDTO()
        {
            Products = new HashSet<ProductDTO>();
        }

        public long Id { get; set; }
        public double Discount { get; set; }
        public int Discount_Level { get; set; }
        public long ProductID { get; set; }
        public int PnaVendorID { get; set; }
        public virtual PnaVendorDTO PnaVendor { get; set; }
        public virtual ICollection<ProductDTO> Products { get; set; }
    }

    public class DiscountLevelDTOView
    {
        public double Discount { get; set; }
        public int Discount_Level { get; set; }
        public long ProductID { get; set; }
        public int PnaVendorID { get; set; }
    }
}