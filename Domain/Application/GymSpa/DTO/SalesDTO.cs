using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Application.GymSpa.DTO
{
    public class SalesDTO
    {
        public SalesDTO()
        {
            Orders = new OrderDTO(); PnaVendor = new PnaVendorDTO();
        }

        public long Id { get; set; }
        public long OrderId { get; set; }
        public virtual OrderDTO Orders { get; set; }
        public int PnaVendorID { get; set; }

        public virtual PnaVendorDTO PnaVendor { get; set; }
        public DateTime DateSoled { get; set; }
    }

    public class SalesDTOView
    {
        [Required]
        public long OrderId { get; set; }

        [Required]
        public int PnaVendorID { get; set; }

        [Required]
        public DateTime DateSoled { get; set; }
    }
}