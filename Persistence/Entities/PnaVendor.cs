using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities
{
    [Table("DB_A57DC4_pnaDb_Vendor")]
    public class PnaVendor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string companyName { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Companyaddress { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Businesscategory { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Accountnumber { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Accountname { get; set; }

        public double Originalserviceamount { get; set; }
        public double Discountrate { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string AccountManagerName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string AccountManagerPhone { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string BankName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string TaxID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string RCnumber { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Vendor_code { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Account_creation_date { get; set; }

        public string BankCode { get; set; }
        public virtual ICollection<EventTicket> EventTickets { get; set; }
        public virtual ICollection<Event>  Events { get; set; }
        public virtual ICollection<EventTicketManager>   EventTicketManagers { get; set; }
        public virtual ICollection<EmergencyTicketManager> EmergencyTicketManagers { get; set; }
    }
}