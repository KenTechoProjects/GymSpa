using AutoMapper;
using Domain.Application.GymSpa.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace APICore.Application.GymSpa.Services
{

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ProductDTOView, ProductDTO>();
            CreateMap<ProductDTO, ProductDTOView>();

            CreateMap<DurationDTOView[], WorkingDateDTO[]>();

            CreateMap<WorkingDateDTO[], DurationDTOView[]>();

            CreateMap<DurationDTOView, WorkingDateDTO>();
            CreateMap<WorkingDateDTO, DurationDTOView>();


            CreateMap<AppointmentDTOView, AppointmentDTO>();
            CreateMap<AppointmentDTO, AppointmentDTOView>();

            CreateMap<DiscountLevelDTOView, DiscountLevelDTO>();
            CreateMap<DiscountLevelDTO, DiscountLevelDTOView>();

            CreateMap<DurationDTOView, DurationDTO>();
            CreateMap<DurationDTO, DurationDTOView>();

            CreateMap<DutyRoasterDTOView, DutyRoasterDTO>();
            CreateMap<DutyRoasterDTO, DutyRoasterDTOView>();

            
            CreateMap<ErrorLogDbDTOView, ErrorLogDbDTO>();
            CreateMap<ErrorLogDbDTO, ErrorLogDbDTOView>();


            CreateMap<ErrorLogSolutionDTOView, ErrorLogSolutionDTO>();
            CreateMap<ErrorLogSolutionDTO, ErrorLogSolutionDTOView>();


            CreateMap<OrderDetailDTOView, OrderDetailDTO>();
            CreateMap<OrderDetailDTO, OrderDetailDTOView>();


            CreateMap<OrderView, OrderDTO>(); 
            CreateMap<OrderDTO, OrderView>();

            CreateMap<OrderDTORequest, OrderDTO>();
            CreateMap< OrderDTO, OrderDTORequest>();

            CreateMap<OrderDTORequests, OrderDTO>();
            CreateMap<OrderDTO, OrderDTORequests>();

            CreateMap<PnaVendorServiceDTOView, PnaVendorServiceDTO>();
            CreateMap<PnaVendorServiceDTO, PnaVendorServiceDTOView>(); 

            CreateMap<ProductCategoryDTO, ProductCategoryDTOView>();
            CreateMap<ProductCategoryDTOView, ProductCategoryDTO>();


            CreateMap<SalesDTOView, SalesDTO>();
            CreateMap<SalesDTO, SalesDTOView>();

            CreateMap<StaffDTOView, StaffDTO>();
            CreateMap<StaffDTO, StaffDTOView>();


            CreateMap<StockDTO, StockDTOView>();
            CreateMap<StockDTOView, StockDTO>();

            CreateMap<BaseServiceDTOView, BaseServiceDTO>();
            CreateMap<BaseServiceDTO, BaseServiceDTOView>();

            CreateMap<ReceiptDTO, ReceiptDTOView>();
            CreateMap<ReceiptDTOView, ReceiptDTO>();


           
        }
    }
}
