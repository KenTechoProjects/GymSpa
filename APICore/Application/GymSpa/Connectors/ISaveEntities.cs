using Domain.Application.EventTickets.DTO;
using Domain.Application.GymSpa.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APICore.Application.GymSpa.Connectors
{
    public interface ISaveEntities
    {
        Task<AppointmentDTO> CreateAppointment(AppointmentDTO model);
        Task<WorkingDateDTO> CreateWorkingDate(WorkingDateDTO model);
        Task<DiscountLevelDTO> CreateDiscountLevel(DiscountLevelDTO model);
        Task<OrderDTO> CreateOrder(OrderDTO model);
        Task<OrderDetailDTO> CreateOrderDetail(OrderDetailDTO model);
        Task<ProductDTO> CreateProduct(ProductDTO model);
        Task<object> CreateProductCategory(ProductCategoryDTO model);
        Task<SalesDTO> CreateSales(SalesDTO model);
        Task<StockDTO> CreateStock(StockDTO model);
        Task<ErrorLogDbDTO> CreateErrorLogDb(ErrorLogDbDTO model);
        Task<StaffDTO> CreateStaff(StaffDTO model);
        Task<DutyRoasterDTO> CreateDutyRoaster(DutyRoasterDTO model);
        Task<ErrorLogSolutionDTO> CreateErrorLogSolution(ErrorLogSolutionDTO model);
        Task<DurationDTO> CreateDuration(DurationDTO model);
        Task<BaseServiceDTO> CreateServices(BaseServiceDTO model);
        Task<EventDTO> CreateEvent(EventDTO model);
        Task<EventTicketDTO> CreateEventTicket(EventTicketDTO model);
        Task<EventTicketManagerDTO> CreateEventTicketManager(EventTicketManagerDTO model);
        Task<EmergencyTicketManagerDTO> CreateEmergencyEventTicketManager(EmergencyTicketManagerDTO model);
        Task<PnaVendorServiceDTO> JoinVendorToService(PnaVendorServiceDTO model);

    }
}
