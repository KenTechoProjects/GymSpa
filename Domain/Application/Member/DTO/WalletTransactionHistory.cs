using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class WalletTransactionHistory
    {
        [Required]
        public string WalletId { get; set; }
    }
}