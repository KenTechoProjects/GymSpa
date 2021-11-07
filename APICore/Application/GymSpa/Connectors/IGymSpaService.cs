 
 
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
 
 
 
using Persistence.Entities;
using Domain.Application.GymSpa.DTO;
using Domain.Application.Univsersal;
using Domain.Models;
 
using APICore.Application.BaseService.Connectors;
using Domain.Enums;

namespace APICore.Application.GymSpa.Connectors
{
  public  interface IGymSpaService<T1>: IGeneralService, IBService<T1> where T1:class
    {

       /// <summary>
       /// Used to get all the service providers
       /// </summary>
       /// <returns></returns>
        Task<T1> GetServiceProviders();
        /// <summary>
        /// Used to get all the service providers by vendor
        /// </summary>
        /// <param name="vendorcode"></param>
        /// <returns></returns>
        Task<T1> Getallservice_by_vendor(string vendorcode);
        /// <summary>
        /// Used to get all the services available
        /// </summary>
        /// <returns></returns>
        Task<T1> Getallservice();
      /// <summary>
      /// Used to book appointments
      /// </summary>
      /// <param name="appointment"></param>
      /// <returns></returns>
        Task<T1> BookAppointment(AppointmentDTO  appointment);
        /// <summary>
        /// Used to get all the staff of a service providers
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> GetClientsStaff (string  vendorCode,string staffId)  ;

        /// <summary>
        /// Upload any kind of file and return the path to be saved to the Database for further processing
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <param name="uploadType"></param>
        /// <returns></returns>
        Task<T1> UploadFile(IFormFile file,string fileName, UploadType uploadType);
        Task<ServerResponse> UploadImage(IFormFile file, string fileName, UploadType uploadType, long productId=0, int vendorId=0);
        /// <summary>
        /// use dto Get total cost Appointment 
        /// </summary>     
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> TotalCost (List<OrderDTO> entity);
        /// <summary>
        /// Payment for service offered to customer
        /// </summary>       
        /// <returns></returns>
        Task<T1> PaymentForService(OderRequest oderRequest, List<ProductDTO> products) ;
        /// <summary>
        /// For printing of any type of receipt for with order details
        /// </summary>
         /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> PrintReceipt(PrintReceiptRequest printOrder);
        /// <summary>
        /// Used to upload the staff nicknames
        /// </summary>
             /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> UplaodStaffNames(IFormFile file);
        /// <summary>
        /// Used to check the status of receipt, if it has been used or not
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> CheckStatusOfReceipt(ObjectNameDTO entity);
        /// <summary>
        /// Used to view the Sales records and commissions
        /// </summary>
           /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> PreviewSalesrecordsAndCommission(DateTime date, string vendorCode);
       // Task<T1> PreviewSalesrecordsAndCommission( string vendorCode, string product=null);
        /// <summary>
        /// Used to set the working days and times
        /// </summary>
          /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> SetWorkingDaysandTime(WorkingDateDTO[]  dates) ;
        /// <summary>
        /// Used to approve Appointments
        /// </summary>
            /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> ApproveAppointment(long apointmentId  ,int pnaVendorID) ;
    
        /// <summary>
        /// Used to approve Appointments
        /// </summary>
           /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> UpdateAppointment(AppointmentDTO appointment, AppointmentColumnUpdate columnName);
        /// <summary>
        /// <summary>
        /// Uzed to cancel approvement
        /// </summary>
           /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> CancelAppointment(long  appointment, string vendorCode);
        /// <summary>
        /// Used to get the discount level of an appoint cost
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T1> GetDiscountLevel(long  productId) ;
        /// <summary>
        /// Used to get the available dates
        /// </summary>
        /// <returns></returns>
        Task<T1> AvailableDates(string vendorCode) ;
        Task<T1> AvailableStaffs(int vendorId) ;
        //Task<T2> AutoMappingObjs<T2, T3>(T2 source, T3 destination);
        Task<T1> TotalCostOfAnAppointment(int vendorId, int memberId, DateTime appointmentdate ) ;
        /// <summary>
        /// Used to get the available Times
        /// </summary>
        /// <returns></returns>
        // Task<ServerResponse> ServerResponses();
        Task<ServerResponse> SendEmailmailnotification(Orderto_pna_admin_emailreqNew request);
        [Obsolete("Not in use ")]
        Task<ServerResponse> Send_emailnotification_pnaadmin(Orderto_pna_admin_emailreq orderto_Pna_Admin_Emailreq);

        Task<ServerResponse> GetMemberOrder(int vendorId, long orderId, string memberId, DateTime orderDate);
        Task<ServerResponse> GetMemberOrderDetail(long orderId, string memberId, DateTime orderDate);
        Task<ServerResponse> GetMemberOrderAmount(IEnumerable<OrderDetailDTO> orderDetailCollection);
    }
}
