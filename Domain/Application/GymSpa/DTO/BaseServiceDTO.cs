using System.Collections.Generic;

namespace Domain.Application.GymSpa.DTO
{
    public partial class BaseServiceDTO
    {
        public long BaseServiceId { get; set; }
        public string ServiceName { get; set; }
        public virtual ICollection<PnaVendorServiceDTO> VendorServices { get; set; }
    }

    public partial class BaseServiceDTOView
    {
        public long BaseServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}