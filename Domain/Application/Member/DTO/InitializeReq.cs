using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class InitializeReq
    {
        [Required]
        public string paymentReference { get; set; }
    }
}