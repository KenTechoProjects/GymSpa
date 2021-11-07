using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Application.Member.DTO
{
    public class MemberauthDTO
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string PhoneNumbers { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string DateofBirth { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Gender { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string ProfilePicture { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string CountryofResidence { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Profession { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(Max)")]
        public string TellUsAboutYourself { get; set; }

        [Column(TypeName = "nvarchar(Max)")]
        public string WhyDoYouWantToJoin { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ReferralCode { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string WalletId { get; set; }

        public string MemberTypeId { get; set; }
        public string QRcode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string subscriptionsStatus { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Alias { get; set; }

        public bool TransactionPinSetupSatus { get; set; }
        public string ActivationCode { get; set; }
        public bool IsActivated { get; set; }
    }
}