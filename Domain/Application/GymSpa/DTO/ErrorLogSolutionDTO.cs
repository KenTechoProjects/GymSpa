using System;

namespace Domain.Application.GymSpa.DTO
{
    public partial class ErrorLogSolutionDTO
    {
        public long Id { get; set; }
        public long? ErrorLogDbId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Solution { get; set; }

        public virtual ErrorLogDbDTO ErrorLogDb { get; set; }
    }

    public partial class ErrorLogSolutionDTOView
    {
        public long? ErrorLogDbId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Solution { get; set; }

        public virtual ErrorLogDbDTO ErrorLogDb { get; set; }
    }
}