using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("DB_A57DC4_pnaDb_sys_logs")]
    public class sys_logs
    {
        public int Id { get; set; }
        public DateTime logDate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(60)")]
        public string Usertype { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(60)")]
        public string channel { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(300)")]
        public string description { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(60)")]
        public string UserID { get; set; }
    }
}