using Domain.Application.EventTickets.DTO;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Application.GymSpa.DTO
{
    public class StaffDTO
    {
        public long Id { get; set; }
        public string StaffID { get; set; }
        public string VendorCode { get; set; }
        public long IncreamentedId { get; set; }
        public int VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendorDTO PnaVendor { get; set; }

        public string Title { get; set; }

        public string FullName { get; set; }

        public string PhoneNumbers { get; set; }

        public DateTime DateofBirth { get; set; }

        public string Gender { get; set; }

        public string ProfilePicture { get; set; }

        public string CountryofResidence { get; set; }

        public string Profession { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string ReferralCode { get; set; }

        public string WalletId { get; set; }

        public string QRcode { get; set; }

        public string MaritalStatus { get; set; }

        public string ActivationCode { get; set; }

        [DefaultValue(false)]
        public bool IsActivated { get; set; }

        public IFormFile ProfileImage { get; set; }
        public ICollection<DutyRoasterDTO> DutyRoasterDTOs { get; set; }
        public virtual ICollection<EventTicketManagerDTO> EventTicketManagers { get; set; }
    }

    public class StaffDTOView
    {
        [Required]
        public string StaffID { get; set; }

        [Required]
        public string VendorCode { get; set; }

        [Required]
        public int VendorId { get; set; }

        //[Required]
        //public string Title { get; set; }
        //[Required]

        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        //[JsonIgnore]
        //public string ProfilePicture { get; set; }
        //[Required]

        //public string CountryofResidence { get; set; }
        //[Required]
        //public string Profession { get; set; }
        //[Required]
        //public string State { get; set; }
        //[Required]
        //public string City { get; set; }
        //[Required]
        public string Email { get; set; }

        //[Required]
        //public string ReferralCode { get; set; }

        //public string WalletId { get; set; }

        //public string QRcode { get; set; }
        //[Required]
        //public string MaritalStatus { get; set; }

        //  public string ActivationCode { get; set; }
        [DefaultValue(false)]
        public bool IsActivated { get; set; }

        [Required]
        public IFormFile ProfileImage { get; set; }

        [Required]
        [Display(Name = "Type of file[Document(1) Product(2)]")]
        public UploadType UploadType { get; set; }
    }
}