using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Application.GymSpa.DTO
{
    public class ReceiptDTO
    {
        public int Id { get; set; }

        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionReference { get; set; }

        public string ServiceCategoty { get; set; }
        public long? OrderId { get; set; }

        public OrderDTO Order { get; set; }

        public int PnaMemberId { get; set; }
        public int VendorId { get; set; }

        public virtual PnaVendorDTO Vendor { get; set; }
    }

    public partial class ReceiptDTOView
    {
        public int Id { get; set; }

        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionReference { get; set; }

        public string ServiceCategoty { get; set; }
        public long OrderId { get; set; }

        public int PnaMemberId { get; set; }
        public int VendorId { get; set; }
    }
}