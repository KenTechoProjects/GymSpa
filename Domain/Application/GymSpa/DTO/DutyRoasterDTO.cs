using System;

namespace Domain.Application.GymSpa.DTO
{
    public class DutyRoasterDTO
    {
        public long Id { get; set; }
        public string StaffId { get; set; }
        public virtual StaffDTO Staff { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VendorId { get; set; }

        public virtual PnaVendorDTO PnaVendor { get; set; }
        public string RequestorId { get; set; }
        public virtual PnaVendorDTO PnaVendorRequestor { get; set; }
    }

    public class DutyRoasterDTOView
    {
        public string StaffId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VendorId { get; set; }

        public string RequestorId { get; set; }
    }
}