using Domain.Application.GymSpa.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APICore.Application.BaseService.Connectors
{/// <summary>
/// The interface that connects for every abstarct implementation
/// </summary>
 
 public  interface IGeneralService
    {
        Task<ServerResponse> CreateOrder (OrderDTORequest order,object products)  ;
        Task<ServerResponse> EditOrder (object order,long orderId)  ;
        Task<ServerResponse> DeleteOrder (long orderId,object type)  ;
        Task<ServerResponse> OrderPaymentReceipt (ReceiptDTO receipt)  ;
        Task<ServerResponse> OrderNotification (object items)  ;
    }
}
