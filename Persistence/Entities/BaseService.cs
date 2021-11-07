using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistence.Entities
{
    [Table(nameof(BaseService))]
 public partial   class BaseService
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BaseServiceId { get; set; }
        public string ServiceName { get; set; } 
   
        public virtual  ICollection<PnaVendorService>  VendorServices { get; set; }


    }
}
