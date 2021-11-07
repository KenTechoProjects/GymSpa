using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("DB_A57DC4_pnaDb_SuperAdmin")]
    public class PnaSuperAdmin
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string username { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string fullname { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string phonenumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(400)")]
        public string password { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string user_type { get; set; }

        public string status { get; set; }
        public DateTime last_login { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        public DateTime creationdate { get; set; }
        public int roleid { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string role { get; set; }

        public DateTime last_activity { get; set; }
        public string autologin { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string notes { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Token { get; set; }

        public DateTime TokenExpiry { get; set; }
        public string SoftToken { get; set; }
        public string profile_pic { get; set; }
    }
}