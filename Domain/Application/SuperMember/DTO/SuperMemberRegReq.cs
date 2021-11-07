using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperMember.DTO
{
    public class SuperMemberRegReq
    {
        [Required]
        public string AdminSoftToken { get; set; }

        [Required]
        public int AdminUserID { get; set; }

        [Required]
        public string user_type { get; set; }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}