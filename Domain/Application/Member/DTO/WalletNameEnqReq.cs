using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class WalletNameEnqReq
    {
        [Required]
        public string WalletId { get; set; }
    }
}