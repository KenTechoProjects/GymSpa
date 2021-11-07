using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class ChangePasswordReq
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Current_Password { get; set; }

        [Required]
        public string New_Password { get; set; }

        [Required]
        public string Confirm_Password { get; set; }
    }
}