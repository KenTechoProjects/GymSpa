using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Application.SuperMember.DTO
{
    public class userAuthTableResponse
    {
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string PhoneNumber { get; set; }

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

        public string AccountCreateDate { get; set; }
    }
}