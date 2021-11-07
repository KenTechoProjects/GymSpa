using System.ComponentModel.DataAnnotations;

namespace Domain.Application.SuperMember.DTO
{
    public class SupermemberLoginReq
    {
        [Required]
        public string phonemail { get; set; }

        [Required]
        public string password { get; set; }
    }
}