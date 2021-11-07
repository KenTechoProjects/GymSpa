using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table(nameof(Duration))]
    public class Duration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public bool IsAvailable { get; set; }
        public bool IsClosed { get; set; }
        public bool IsActive { get; set; }
        public string VendorCode { get; set; }

        //public long? AppointmentId { get; set; }
        //[ForeignKey(nameof(AppointmentId))]
        //public virtual GymSpaAppointment Appointment { get; set; }
        public int? PnaVendorID { get; set; }

        [ForeignKey(nameof(PnaVendorID))]
        public virtual PnaVendor PnaVendor { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "nvarchar(15)")]
        public DateTime? AlotedDate { get; set; }
    }
}