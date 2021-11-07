using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("DB_A57DC4_pnaDb_SuperMemberAuth")]
    public class PnaSuperMemberAuth
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(22)")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(400)")]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Token { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public DateTime TokenExpiry { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string LastLoginDate { get; set; }

        [Column(TypeName = "nvarchar(60)")]
        public string PasswordChangeDate { get; set; }
    }
}