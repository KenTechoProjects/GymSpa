using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    public partial class Staff
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Key]
        public string StaffID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string VendorCode { get; set; }

        public long IncreamentedId { get; set; }
        public int? VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public virtual PnaVendor PnaVendor { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string PhoneNumbers { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? DateofBirth { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Gender { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string ProfilePicture { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string CountryofResidence { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Profession { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string State { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string City { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ReferralCode { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string WalletId { get; set; }

        public string QRcode { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string MaritalStatus { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string ActivationCode { get; set; }

        public bool IsActivated { get; set; }
        public bool IsAvailable { get; set; }
        public virtual ICollection<EventTicketManager> EventTicketManagers { get; set; }
    }
}