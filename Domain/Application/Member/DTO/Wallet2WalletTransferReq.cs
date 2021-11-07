using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class Wallet2WalletTransferReq
    {
        [Required]
        public string SenderWalletId { get; set; }

        [Required]
        public string TransactionPin { get; set; }

        [Required]
        public string ReceiverWalletId { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Narration { get; set; }
    }
}