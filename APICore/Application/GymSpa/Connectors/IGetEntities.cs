using Domain.Application.EventTickets.DTO;
using Domain.Application.GymSpa.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICore.Application.GymSpa.Connectors
{
    public interface IGetEntities
    {
        Task<IEnumerable<ErrorLogDbDTO>> GetErrorLogDbd(long Id, int vendorId);
        Task<IEnumerable<AppointmentDTO>> GetAppointmentd(long Id, int vendorId);
        Task<IEnumerable<DiscountLevelDTO>> GetDiscountLeveld(long Id, int vendorId);
        Task<IEnumerable<ErrorLogSolutionDTO>> GetErrorLogSolutiond(long Id, long errorLogDbId);
        Task<IEnumerable<OrderDTO>> GetOrderd(long Id, int vendorId, string membershipId);
        Task<IEnumerable<OrderDTO>> GetOrderd(long Id, int vendorId);
        Task<IEnumerable<OrderDetailDTO>> GetOrderDetaild(long Id, long orderId, int vendorId);
        Task<IEnumerable<ProductDTO>> GetProductd(long Id, long categoryId, string vendorCode);
        Task<IEnumerable<ProductCategoryDTO>> GetProductCategoryd(long Id, int vendorId);
        Task<IEnumerable<SalesDTO>> GetSale(long Id, long orderId, int vendorId);
        Task<IEnumerable<StaffDTO>> GetStaffd(string staffId, string vendorCode);
        Task<IEnumerable<DurationDTO>> GetDuration(long Id, int vendorId, DateTime dateAloted);
        Task<IEnumerable<StockDTO>> GetStockd(long Id, int vendorId);

        Task<IEnumerable<DutyRoasterDTO>> GetDutyRoaster(long Id, int vendorId, string staffId);
        Task<IEnumerable<WorkingDateDTO>> GetWorkingDated(long Id, string vendorCode);

        Task<IEnumerable<ErrorLogDbDTO>> GetErrorLogDbds();
        Task<IEnumerable<AppointmentDTO>> GetAppointmentds();
        Task<IEnumerable<DiscountLevelDTO>> GetDiscountLevelds();
        Task<IEnumerable<ErrorLogSolutionDTO>> GetErrorLogSolutionds();
        Task<IEnumerable<OrderDTO>> GetOrderds();
        Task<IEnumerable<OrderDetailDTO>> GetOrderDetailds();
        Task<IEnumerable<ProductDTO>> GetProductds();
        Task<IEnumerable<ProductCategoryDTO>> GetProductCategoryds();
        Task<IEnumerable<SalesDTO>> GetSales();
        Task<IEnumerable<StaffDTO>> GetStaffds();
        Task<IEnumerable<StockDTO>> GetStockds();
        Task<IEnumerable<WorkingDateDTO>> GetWorkingDateds();
        Task<IEnumerable<DutyRoasterDTO>> GeDutyRoasters();
        Task<IEnumerable<DurationDTO>> GetDurations();


        Task<EventDTO> GetEvent(long Id, int vendorId);
        Task<EventDTO> GetEvent(long Id);
        Task<IEnumerable<EventDTO>> GetEvent(int vendorId, int EventTicketManagerId = 0, string itemCode = null);
       
        Task<EventTicketDTO> GetEventTicket(long Id, int vendorId);
        Task<IEnumerable<EventTicketDTO>> GetEventTickets(int vendorId);
        /// <summary>
        /// Get using vendorId or membership or ticketdate or eventticketmanager
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="membershipId"></param>
        /// <param name="dateofTicket"></param>
        /// <param name="EventTicketManagerId"></param>
        /// <returns></returns>
        Task<IEnumerable<EventTicketDTO>> GetEventTickets(int vendorId, int membershipId=0, DateTime? dateofTicket=null, int EventTicketManagerId=0);




        Task<EventTicketManagerDTO> GetEventTicketManager(long id);
        Task<IEnumerable<EventTicketManagerDTO>> GetEventTicketManagers(long Id, int vendorId, long staffId);    
        Task<IEnumerable<EventTicketManagerDTO>> GetEventTicketManager(string managerCode, int vendorId, long staffId, long ticketManagerId);

         
        Task<EmergencyTicketManagerDTO> GetEmergencyTicketManager(long id);
        Task<EventTicketManagerDTO> GetEmergencyTicketManagers(long Id, int vendorId); 
        Task<IEnumerable<EventTicketManagerDTO>> GetEmergencyTicketManagers(string managerCode, int vendorId, long ticketManagerId);


    }
}