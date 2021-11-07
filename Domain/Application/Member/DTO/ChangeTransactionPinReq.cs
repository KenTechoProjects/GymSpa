using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class ChangeTransactionPinReq
    {
        [Required]
        public string WalletId { get; set; }

        [Required]
        public string Old_TransactionPin { get; set; }

        [Required]
        public string New_TransactionPin { get; set; }
    }
}