using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table(nameof(ErrorLogDb))]
    public partial class ErrorLogDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Exception { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateResolved { get; set; }
        public string Class_ { get; set; }
        public string Method { get; set; }

        public virtual ICollection<ErrorLogSolution> ErrorLogSolutions { get; set; }
    }
}