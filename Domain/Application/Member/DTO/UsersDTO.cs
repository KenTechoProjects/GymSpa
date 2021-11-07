using Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Application.Member.DTO
{
    public class UsersDTO
    {
        [Column(TypeName = "nvarchar(100)")]
        public string WalletId { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; }

        public List<FundwalletDetails> fundwalletHistory { get; set; }

        public List<WalletTransferHistoryDTO> walletTransferHistory { get; set; }
    }
}