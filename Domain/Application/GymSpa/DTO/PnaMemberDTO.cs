using System.Collections.Generic;
using System.ComponentModel;

namespace Domain.Application.GymSpa.DTO
{
    public class PnaMemberDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FullName { get; set; }

        public string PhoneNumbers { get; set; }

        public string DateofBirth { get; set; }

        public string Gender { get; set; }

        public string ProfilePicture { get; set; }

        public string CountryofResidence { get; set; }

        public string Profession { get; set; }

        public string Email { get; set; }

        public string TellUsAboutYourself { get; set; }

        public string WhyDoYouWantToJoin { get; set; }

        public string ReferralCode { get; set; }

        public string WalletId { get; set; }
        public string MemberTypeId { get; set; }
        public string QRcode { get; set; }

        public string subscriptionsStatus { get; set; }

        public string Alias { get; set; }
        public string TransactionPin { get; set; }

        [DefaultValue(false)]
        public bool TransactionPinSetupSatus { get; set; }

        public string MembershipID { get; set; }

        public string Status { get; set; }
        public string ActivationCode { get; set; }

        [DefaultValue(false)]
        public bool IsActivated { get; set; }

        public virtual ICollection<AppointmentDTO> Appointments { get; set; }
        public virtual ICollection<EventTicketDTO>  EventTickets { get; set; }
    }
}