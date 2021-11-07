using Domain.Application.EventTickets.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Application.GymSpa.DTO
{
    //[Table("ServiceProvider")]
    public class PnaVendorDTO
    {
        public PnaVendorDTO()
        {
            Appointments = new HashSet<AppointmentDTO>();
            Orders = new HashSet<OrderDTO>();
            ProductCategories = new HashSet<ProductCategoryDTO>();
            Sales = new HashSet<SalesDTO>();
            Stocks = new HashSet<StockDTO>();
            DiscountLevels = new HashSet<DiscountLevelDTO>();
            DutyRoasterForStaffs = new HashSet<DutyRoasterDTO>();
            DutyRoasterForRequestors = new HashSet<DutyRoasterDTO>();
            Receipts = new HashSet<ReceiptDTO>();
            EventTickets = new HashSet<EventTicketDTO>();
            EventTicketManagers = new HashSet<EventTicketManagerDTO>();
        }

        public int Id { get; set; }

        [Required]
        public string companyName { get; set; }

        public string Companyaddress { get; set; }

        public string Email { get; set; }

        public string Businesscategory { get; set; }

        public string Accountnumber { get; set; }

        public string Accountname { get; set; }

        public double Originalserviceamount { get; set; }
        public double Discountrate { get; set; }

        public string AccountManagerName { get; set; }

        public string AccountManagerPhone { get; set; }

        public string BankName { get; set; }

        public string TaxID { get; set; }

        public string RCnumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Vendor_code { get; set; }

        public DateTime Account_creation_date { get; set; }
        public string BankCode { get; set; }
        public virtual ICollection<ProductCategoryDTO> ProductCategories { get; set; }
        public virtual ICollection<AppointmentDTO> Appointments { get; set; }
        public virtual ICollection<OrderDTO> Orders { get; set; }
        public virtual ICollection<SalesDTO> Sales { get; set; }
        public virtual ICollection<StockDTO> Stocks { get; set; }
        public virtual ICollection<WorkingDateDTO> AvailableDate { get; set; }
        public virtual ICollection<PnaVendorServiceDTO> VendorServices { get; set; }
        public virtual ICollection<DiscountLevelDTO> DiscountLevels { get; set; }

        [InverseProperty("VendorCode")]
        public virtual ICollection<DutyRoasterDTO> DutyRoasterForStaffs { get; set; }

        [InverseProperty("RequestorCode")]
        public virtual ICollection<DutyRoasterDTO> DutyRoasterForRequestors { get; set; }

        public virtual ICollection<DurationDTO> Durations { get; set; }
        public virtual ICollection<ReceiptDTO> Receipts { get; set; }
        public virtual ICollection<EventTicketManagerDTO> EventTicketManagers { get; set; }
        public virtual ICollection<EventTicketDTO>  EventTickets { get; set; }
         

    }
}