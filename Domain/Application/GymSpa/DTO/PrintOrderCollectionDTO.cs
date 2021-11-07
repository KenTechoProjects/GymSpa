using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Application.GymSpa.DTO
{
    public class PrintOrderCollectionDTO
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
    }

    public class PrintOrderReturnedDTO
    {
        public List<PrintOrderCollectionDTO> ListItemPriceAndQuantity { get; set; }
        public string ServiceCategory { get; set; }
        public string OrderReference { get; set; }
        public string TransactionReference { get; set; }

        [DataType(DataType.Date)]
        public DateTime Transactiondate { get; set; }
    }
}