using System.ComponentModel.DataAnnotations;

namespace Domain.Application.Member.DTO
{
    public class RegReq
    {
        [Required]
        public string referralCode { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Alias { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string DateofBirth { get; set; }

        public string Gender { get; set; }

        public string CountryofResidence { get; set; }

        public string Profession { get; set; }

        public string TellUsAboutYourself { get; set; }

        public string WhyDoYouWantToJoin { get; set; }
        public string MemberTypeId { get; set; }
    }
}