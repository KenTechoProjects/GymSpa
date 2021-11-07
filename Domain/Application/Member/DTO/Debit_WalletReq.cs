using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class Debit_WalletReq
    {
        [Required]
        public string WalletId { get; set; }

        [Required]
        public double amount { get; set; }

        [Required]
        public string TransactionPin { get; set; }
    }
}