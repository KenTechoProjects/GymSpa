using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperAdmin
{
    public class SuperAdminUpdateAdminstaffProfileReq
    {
        [Required]
        public string superAdminSoftToken { get; set; }

        [Required]
        public int superAdminUserID { get; set; }

        [Required]
        public int AdminstaffID { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string fullname { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(16, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 11)]
        public string phonenumber { get; set; }

        [Display(Name = "NEW Password")]
        [StringLength(12, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        public string password { get; set; }

        [Display(Name = "Comfirm Password")]
        [StringLength(12, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        public string confirmpassword { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public IFormFile profile_pic { get; set; }
    }
}