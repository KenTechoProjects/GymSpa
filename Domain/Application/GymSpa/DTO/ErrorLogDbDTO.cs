using System;
using System.Collections.Generic;

namespace Domain.Application.GymSpa.DTO
{
    public partial class ErrorLogDbDTO
    {
        public long Id { get; set; }
        public string Exception { get; set; }
        public string Class_ { get; set; }
        public string Method { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateResolved { get; set; }
        public int? PnaVendorID { get; set; }

        public virtual PnaVendorDTO PnaVendor { get; set; }
        public virtual ICollection<ErrorLogSolutionDTO> ErrorLogSolutions { get; set; }
    }

    public partial class ErrorLogDbDTOView
    {
        public string Exception { get; set; }
        public string Class_ { get; set; }
        public string Method { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateResolved { get; set; }
        public int? PnaVendorID { get; set; }

        public virtual PnaVendorDTO PnaVendor { get; set; }
        public virtual ICollection<ErrorLogSolutionDTO> ErrorLogSolutions { get; set; }
    }
}