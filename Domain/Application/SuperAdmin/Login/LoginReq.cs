using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperAdmin.Login
{
    public class LoginReq
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}