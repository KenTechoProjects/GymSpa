using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Application.GymSpa.DTO
{
    public class DurationDTO
    {
        public long Id { get; set; }

        [DataType(DataType.Time)]
        [Required]
        public DateTime Time { get; set; }

        [DataType(DataType.Date)]
        public DateTime AlotedDate { get; set; }

        [DefaultValue(false)]
        public bool IsAvailable { get; set; }

        [DefaultValue(false)]
        public bool IsClosed { get; set; }

        public bool IsActive { get; set; }

        [ScaffoldColumn(false)]
        public string VendorCode { get; set; }

        [Required]
        public int PnaVendorID { get; set; }

        public virtual PnaVendorDTO PnaVendor { get; set; }
        //public long? AppointmentId { get; set; }

        //public virtual AppointmentDTO Appointment { get; set; }
    }

    public class DurationDTOView
    {
        [DataType(DataType.Time)]
        [Required]
        public DateTime Time { get; set; }

        [DataType(DataType.Date)]
        public DateTime AlotedDate { get; set; }

        public bool IsAvailable { get; set; }
        public bool IsClosed { get; set; }
        public bool IsActive { get; set; }

        [Required]
        public int PnaVendorID { get; set; }

        //public long? AppointmentId { get; set; }

        //public virtual AppointmentDTO Appointment { get; set; }
    }
}