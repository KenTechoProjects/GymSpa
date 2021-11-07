using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Application.GymSpa.DTO
{
    public class WorkingDateDTO
    {
        public WorkingDateDTO()
        {
            PnaVendor = new PnaVendorDTO();
        }

        public long Id { get; set; }

        [Required]
        public DateTime? Availabledate { get; set; }

        [DefaultValue(false)]

        public bool IsAvailable { get; set; } = false;[DefaultValue(false)]

        public bool IsClosed { get; set; } = false;[DefaultValue(false)]

        public bool IsActive { get; set; } = false;

        [Required]
        public long PnaVendorID { get; set; }

        public virtual PnaVendorDTO PnaVendor { get; set; }
        //    public long AppointmentId { get; set; }

        //    public virtual AppointmentDTO Appointment { get; set; }
    }

    public class WorkingDateDTOView
    {
        public long Id { get; set; }

        [Required]
        public DateTime? Availabledate { get; set; }

        [DefaultValue(false)]
        public bool IsAvailable { get; set; } = false;

        [DefaultValue(false)]
        public bool IsClosed { get; set; } = false;

        [DefaultValue(false)]
        public bool IsActive { get; set; } = false;

        [Required]
        public long PnaVendorID { get; set; }

        //    public long AppointmentId { get; set; }

        //    public virtual AppointmentDTO Appointment { get; set; }
    }
}