namespace Domain.Application.GymSpa.DTO
{
    public class PnaVendorServiceDTO
    {
        public long PnaVendorServiceId { get; set; }
        public long? VendoId { get; set; }
        public long? ServiceId { get; set; }

        public virtual PnaVendorDTO PnaVendor { get; set; }

        public virtual BaseServiceDTO Service { get; set; }
    }

    public class PnaVendorServiceDTOView
    {
        public long PnaVendorServiceId { get; set; }
        public long? VendoId { get; set; }
        public long? ServiceId { get; set; }
    }
}