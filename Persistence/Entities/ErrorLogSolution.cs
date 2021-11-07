using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table(nameof(ErrorLogSolution))]
    public partial class ErrorLogSolution
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long? ErrorLogDbId { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Solution { get; set; }

        [ForeignKey(nameof(ErrorLogDbId))]
        public virtual ErrorLogDb ErrorLogDb { get; set; }
    }
}