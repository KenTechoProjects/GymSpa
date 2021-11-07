using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class payMembershipSubcription
    {
        [Required]
        public string WalletId { get; set; }
    }
}