using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Domain.Application.GymSpa.DTO
{
    public class AppointmentDTO
    {
        public AppointmentDTO()
        {
            PnaVendor = new PnaVendorDTO();
        }

        public long Id { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public string Duration { get; set; }

        [Required]
        public int PnaVendorId { get; set; }

        [ScaffoldColumn(false)]
        public virtual PnaVendorDTO PnaVendor { get; set; }

        public bool Active { get; set; } = false;

        public bool Closed { get; set; } = false;

        public bool Cancelled { get; set; } = false;

        public bool Approved { get; set; } = false;

        [Required]
        public int MemberId { get; set; }

        [ScaffoldColumn(false)]
        //  [JsonIgnore]
        public virtual PnaMemberDTO PnaMember { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public double Discount { get; set; }

        [Required]
        public AppointmentColumnUpdate AppointmentColumnUpdate { get; set; }

        [ScaffoldColumn(false)]
        [JsonIgnore]
        public virtual ICollection<WorkingDateDTO> Dates { get; set; }

        // public virtual ICollection<DurationDTO> Durations { get; set; }
    }

    public class AppointmentDTOView
    {
        [Required]
        [JsonPropertyName("Appointment-Date")]
        [DataMember]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [DataMember]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        [Required]
        [DataMember]
        public string Duration { get; set; }

        [Required]
        [DataMember]
        public int PnaVendorId { get; set; }

        [DataMember]
        [DefaultValue(false)]
        public bool Active { get; set; } = false;

        [DataMember]
        [DefaultValue(false)]
        public bool Closed { get; set; } = false;

        [DataMember]
        [DefaultValue(false)]
        public bool Cancelled { get; set; } = false;

        [DataMember]
        [DefaultValue(false)]
        public bool Approved { get; set; } = false;

        [Required]
        [DataMember]
        public int MemberId { get; set; }

        [Required]
        [DataMember]
        public double Cost { get; set; }

        [Required]
        [DataMember]
        public double Discount { get; set; }

        [Required]
        [DataMember]
        public AppointmentColumnUpdate AppointmentColumnUpdate { get; set; }

        // public virtual ICollection<DurationDTO> Durations { get; set; }
    }
}