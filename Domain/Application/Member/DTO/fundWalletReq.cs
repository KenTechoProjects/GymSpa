using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class fundWalletReq
    {
        [Required]
        public string WalletId { get; set; }

        [Required]
        public double Amount { get; set; }
    }
}