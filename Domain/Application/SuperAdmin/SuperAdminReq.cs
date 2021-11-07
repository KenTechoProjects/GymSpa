using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperAdmin
{
    public class SuperAdminReq
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string TokenKey { get; set; }
    }
}