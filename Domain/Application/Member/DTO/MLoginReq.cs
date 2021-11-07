using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class MLoginReq
    {
        [Required]
        public string phonemail { get; set; }

        [Required]
        public string password { get; set; }
    }
}