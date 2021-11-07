using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class TransactionPinSetupReq
    {
        [Required]
        public string WalletId { get; set; }

        [Required]
        public string New_TransactionPin { get; set; }
    }
}