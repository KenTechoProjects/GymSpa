using APICore.Application.GymSpa.Connectors;
using AppCore.Application.TransactionNotification;
using Dapper;
using Domain.Application.GymSpa.DTO;

using Domain.Application.GymSpa.Extensions;
using Domain.Application.Univsersal;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using Notification.Interface;
using Persistence;
using Persistence.Entities;
using SharedService.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;
using Utilities.Interface;
using Utilities.Service;
using System.Linq;
using Utilities.SystemActivity.Interface;
using System.IO;
//using Microsoft.Extensions.Hosting;
//using Microsoft.AspNetCore.Hosting;
using AutoMapper;

using APICore.Application.BaseService.Connectors;
using System.Globalization;
using Dapper.Contrib.Extensions;
using Domain.DTO.Notification.Email;
using Domain.Application.SystemUtility.Sys_Logs;
using Domain.Enums;
using Domain.Application.EventTickets.DTO;

namespace APICore.Application.GymSpa.Services
{
    public class GymSpaService : IGymSpaService<ServerResponse>
    {

        private readonly Utilities.Logger _logger;
        private readonly AppKeys _appKeys;
        private readonly IConnectionStrings _config;
        private readonly IDapperr _dapper;
        private readonly ISystemActivityService _systemUtility;
        private readonly AppSystemType _appSystemType;
        private readonly AppChannel _appChannel;
        private readonly ISharedService _sharedService;
        private readonly INotificationServices _notificationServices;
        private readonly BaseUrls _baseUrls;
        private readonly UIHttpClient _httpClient;
        private readonly ITransactionNotification _transactionNotification;
        private readonly PNAContext _context;
        private readonly ISaveEntities _saveEntities;
        private readonly IGetEntities _getEntities;
        private readonly IMapper _mapper;
        private string _connectionstr = string.Empty;

        public GymSpaService(Utilities.Logger logger, IOptions<AppKeys> appKeys,
            IConnectionStrings configuration, ISystemActivityService systemUtility,
            IOptions<AppSystemType> appsystemtype, IOptions<AppChannel> channel,
            ISharedService sharedService,
            INotificationServices notificationServices,
            IOptions<BaseUrls> baseUrls, UIHttpClient httpClient,
            ITransactionNotification transactionNotification, PNAContext context, IDapperr dapper, ISaveEntities saveEntities,
            IGetEntities getEntities, IMapper mapper)
        {

            _appKeys = appKeys.Value;
            _logger = logger;
            _config = configuration;
            _systemUtility = systemUtility;
            _appSystemType = appsystemtype.Value;
            _appChannel = channel.Value;
            _sharedService = sharedService;
            _notificationServices = notificationServices;
            _httpClient = httpClient;
            _baseUrls = baseUrls.Value;
            _transactionNotification = transactionNotification;
            _context = context;
            _dapper = dapper;
            _connectionstr = _config.GetConnectionString();
            _saveEntities = saveEntities;
            this._getEntities = getEntities;
            this._mapper = mapper;
        }
        public async Task<ServerResponse> Add(object entity)
        {
            int check = 0;
            try
            {

                dynamic data = null;
                string sql = string.Empty;
                string requestid = CustomFunctions.RequestID();
                if (entity.GetType() == typeof(AppointmentDTO))
                {
                    data = await _saveEntities.CreateAppointment((AppointmentDTO)entity);
                }
                else if (entity.GetType() == typeof(WorkingDateDTO))
                {
                    data = await _saveEntities.CreateWorkingDate((WorkingDateDTO)entity);
                }
                else if (entity.GetType() == typeof(DiscountLevelDTO))
                {
                    data = await _saveEntities.CreateDiscountLevel((DiscountLevelDTO)entity);
                }
                else if (entity.GetType() == typeof(OrderDetailDTO))
                {
                    data = await _saveEntities.CreateOrderDetail((OrderDetailDTO)entity);
                }

                else if (entity.GetType() == typeof(OrderDTO))
                {
                    data = await _saveEntities.CreateOrder((OrderDTO)entity);
                }

                else if (entity.GetType() == typeof(ProductDTO))
                {

                    data = await _saveEntities.CreateProduct((ProductDTO)entity);
                }

                else if (entity.GetType() == typeof(ProductCategoryDTO))
                {
                    var exists = await FileExists(entity);
                    if (exists.Data == null)
                    {
                        data = await _saveEntities.CreateProductCategory((ProductCategoryDTO)entity);
                    }
                    else
                    {
                        data = exists;
                    }


                }

                else if (entity.GetType() == typeof(SalesDTO))
                {



                    data = await _saveEntities.CreateSales((SalesDTO)entity);
                }


                else if (entity.GetType() == typeof(StaffDTO))
                {



                    data = await _saveEntities.CreateStaff((Domain.Application.GymSpa.DTO.StaffDTO)entity);
                }


                else if (entity.GetType() == typeof(ErrorLogDbDTO))
                {
                    data = await _saveEntities.CreateErrorLogDb((ErrorLogDbDTO)entity);
                }
                else if (entity.GetType() == typeof(ErrorLogSolutionDTO))
                {


                    data = await _saveEntities.CreateErrorLogSolution((ErrorLogSolutionDTO)entity);

                }
                else if (entity.GetType() == typeof(DutyRoasterDTO))
                {


                    data = await _saveEntities.CreateDutyRoaster((DutyRoasterDTO)entity);

                }

                else if (entity.GetType() == typeof(DurationDTO))
                {


                    data = await _saveEntities.CreateDuration((DurationDTO)entity);

                }

                else if (entity.GetType() == typeof(StockDTO))
                {


                    data = await _saveEntities.CreateStock((StockDTO)entity);

                }
                else if (entity.GetType() == typeof(BaseServiceDTO))
                {


                    data = await _saveEntities.CreateServices((BaseServiceDTO)entity);

                }
                else if (entity.GetType() == typeof(PnaVendorServiceDTO))
                {


                    data = await _saveEntities.JoinVendorToService((PnaVendorServiceDTO)entity);

                }

                if (data != null && data.Id > 0)
                {

                    if (data.Id > 0)
                    {
                        return new ServerResponse
                        {

                            Data = data,
                            Code = ResponseCodes.SUCCESS,
                            Message = "Record was created successfully",
                            RequestId = requestid,
                            Status = StatusCodesExtended.Status200OK.ToString(),
                            IsSuccessful = true

                        };
                    }
                    else
                    {
                        return new ServerResponse
                        {

                            Data = data,
                            Code = ResponseCodes.UNSUCCESSFUL,
                            Message = "Record was not created successfully",
                            RequestId = requestid,
                            Status = StatusCodesExtended.Status411NotCreated.ToString(),
                            IsSuccessful = false

                        };
                    }
                }
                else
                {
                    return new ServerResponse
                    {

                        Data = data,
                        Code = ResponseCodes.UNSUCCESSFUL,
                        Message = "Record was not created successfully",
                        RequestId = requestid,
                        Status = StatusCodesExtended.Status411NotCreated.ToString(),
                        IsSuccessful = false

                    };
                }


            }
            catch (Exception ex)
            {
                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "Add",
                //    Class_ = "GymSpaServices",

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new reCord][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(entity) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                return new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.ERROR,
                    Message = $"Record was not created successfully: {ex}",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                    IsSuccessful = false

                };
            }
        }

        public async Task<object> Add(ErrorLogDbDTO entity)
        {

            if (entity != null)
            {
                object obj = new object();
                try
                {
                    using (IDbConnection con = new SqlConnection(_connectionstr))
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }

                        var trans = con.BeginTransaction();
                        string sql = "INSERT INTO ErrorLogDb (Exception,DateCreated,PnaVendorID) OUTPUT INSERTED.* VALUES (@Exception,@DateCreated,@PnaVendorID)";

                        obj = await con.QueryFirstOrDefaultAsync<ErrorLogSolutionDTO>(sql, entity, trans);

                    };
                }
                catch (Exception ex)
                {
                    //await Add(new ErrorLogDbDTO
                    //{

                    //    DateCreated = DateTime.Now,
                    //    Exception = ex.ToString(),
                    //    Method = "Add",
                    //    Class_ = "GymSpaServices"

                    //});

                    obj = await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new ReCord][Response] => {ex.Message} | [request]=> " +
                                        JsonConvert.SerializeObject(entity) + $"| [requestId]=> {CustomFunctions.RequestID()}");

                    return new ServerResponse
                    {
                        Data = null,
                        Code = ResponseCodes.ERROR,
                        Message = ex.ToString(),
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                        IsSuccessful = true
                    };
                }
                return obj;
            }
            else
            {
                return new ServerResponse
                {
                    Data = null,
                    Code = ResponseCodes.EMPTYFIELD,
                    Message = "Empty filed is indicated",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                    IsSuccessful = false
                };


            }

        }

        [NotImplemented("Not yet impplemented do not use yet")]
        public Task<ServerResponse> AddRange(Type entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ServerResponse> ApproveAppointment(long apointmentId, int pnaVendorID)
        {
            ServerResponse sr = null;

            try
            {



                using (IDbConnection con = new SqlConnection(_connectionstr))
                {


                    string sql = string.Empty;

                    if (apointmentId > 0 && pnaVendorID > 0)
                    {
                        sql = "Update GymSpaAppointment set Approved=1 where Id=@Id and PnaVendorID=@PnaVendorID;";
                        if (con.State == ConnectionState.Closed)
                        {
                            DynamicParameters paraAdd = new DynamicParameters();
                            paraAdd.Add("Approved", new { Approved = true }, DbType.Boolean);
                            con.Open();
                            var saved = await con.ExecuteAsync(sql); // await con.QueryFirstAsync<Appointment>(sql, appointment);
                            var trans = con.BeginTransaction();
                            if (saved > 0)
                            {
                                trans.Commit();
                                sr = new ServerResponse
                                {

                                    Data = saved,
                                    Code = ResponseCodes.SUCCESS,
                                    IsSuccessful = true,
                                    Message = $"  Appointment was  approved successfully",
                                    Status = StatusCodesExtended.Status211Updated.ToString()
                                };
                            }
                            else
                            {
                                trans.Rollback();
                                sr = new ServerResponse
                                {

                                    Data = null,
                                    Code = ResponseCodes.UNSUCCESSFUL,
                                    IsSuccessful = false,
                                    Message = $" The appointment was not approved successfully and transaction rolled back",
                                    Status = StatusCodesExtended.Status304NotModified.ToString()
                                };
                            }
                        }


                    }



                };

            }
            catch (Exception ex)
            {
                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "Add Appointment",
                //    Class_ = "GymSpaServices"

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new Appointment][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(new { apointmentId, pnaVendorID }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                sr = new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.ERROR,
                    IsSuccessful = false,
                    Message = $" The appointment was not approved successfully: an error occured",
                    Status = StatusCodesExtended.Status500InternalServerError.ToString()
                };
            }

            return await Task.FromResult(sr);
        }

        public async Task<ServerResponse> UpdateAppointment(AppointmentDTO appointment, AppointmentColumnUpdate columnName)
        {
            var sr = new ServerResponse();
            try
            {
                bool active, apdate, time, dura, close, canc, appr = false;
                string message = string.Empty;
                string messageN = string.Empty;
                //DynamicParameters dbparams = new DynamicParameters();
                string requestid = CustomFunctions.RequestID();
                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string sql = string.Empty;
                    if (columnName == AppointmentColumnUpdate.Active)
                    {
                        active = true;
                        message = "Appointment was actived successfully";
                        messageN = "Appointment was not actived successfully";
                        sql = "Update DB_A57DC4_pnaDb_Appointment  SET Active=@Active WHERE PnaVendorId=@PnaVendorId AND Id=@Id;";
                        //  dbparams.Add("Active", appointment.Active, DbType.Boolean);
                    }
                    else if (columnName == AppointmentColumnUpdate.AppointmentDate)
                    {
                        message = "Appointment date updated was  successfully";
                        messageN = "Appointment date was not updated successfully";
                        apdate = true;
                        sql = "Update DB_A57DC4_pnaDb_Appointment  SET AppointmentDate=@AppointmentDate WHERE PnaVendorId=@PnaVendorId AND Id=@Id;";
                        //dbparams.Add("AppointmentDate", appointment.AppointmentDate, DbType.Date);
                    }
                    else if (columnName == AppointmentColumnUpdate.Time)
                    {
                        time = true;
                        message = "Appointment time updated was nsuccessfully";
                        messageN = "Appointment time was not updated successfully";
                        sql = "Update DB_A57DC4_pnaDb_Appointment  SET Time=@Time WHERE PnaVendorId=@PnaVendorId AND Id=@Id;";
                        //dbparams.Add("Time", appointment.Time, DbType.Date);
                    }
                    else if (columnName == AppointmentColumnUpdate.Duration)
                    {
                        dura = true; message = "Appointment Duration was updated successfully";
                        messageN = "Appointment Duration was not updated successfully";
                        sql = "Update DB_A57DC4_pnaDb_Appointment  SET Duration=@Duration WHERE PnaVendorId=@PnaVendorId AND Id=@Id;";
                        //dbparams.Add("Duration", appointment.Duration, DbType.String);
                    }


                    else if (columnName == AppointmentColumnUpdate.Closed)
                    {
                        close = true;

                        message = " Appointment was closed successfully";
                        messageN = " Appointment was not closed successfully";
                        //dbparams.Add("Closed", appointment.Closed, DbType.Boolean);
                        sql = "Update DB_A57DC4_pnaDb_Appointment  SET Closed=@Closed WHERE PnaVendorId=@PnaVendorId AND Id=@Id;";
                    }

                    else if (columnName == AppointmentColumnUpdate.Cancelled)
                    {
                        canc = true;
                        message = " Appointment was  calnelled successfully";
                        messageN = " Appointment was not calnelled successfully";
                        // dbparams.Add("Cancelled", appointment.Cancelled, DbType.Boolean);
                        sql = "Update DB_A57DC4_pnaDb_Appointment  SET Cancelled=@Cancelled WHERE PnaVendorId=@PnaVendorId AND Id=@Id;";
                    }


                    else if (columnName == AppointmentColumnUpdate.Approved)
                    {
                        appr = true;

                        message = " Appointment was approved successfully";
                        messageN = " Appointment was not approved successfully";
                        sql = "Update DB_A57DC4_pnaDb_Appointment  SET Cancelled=@Cancelled WHERE PnaVendorId=@PnaVendorId AND Id=@Id ";
                        // dbparams.Add("Approved", appointment.Approved, DbType.Boolean);
                    }

                    using (var trans = con.BeginTransaction())
                    {
                        var saved = await con.ExecuteAsync(sql, appointment, trans);
                        if (saved > 0)
                        {
                            trans.Commit();
                            sr = new ServerResponse
                            {

                                Data = "Updated",
                                Code = ResponseCodes.SUCCESS,
                                IsSuccessful = true,
                                Message = $"{message}",
                                Status = StatusCodesExtended.Status211Updated.ToString(),
                                RequestId = requestid
                            };
                        }
                        else
                        {
                            trans.Rollback();
                            sr = new ServerResponse
                            {

                                Data = null,
                                Code = ResponseCodes.UNSUCCESSFUL,
                                IsSuccessful = false,
                                Message = $" {messageN} and transaction rolled back",
                                Status = StatusCodesExtended.Status304NotModified.ToString(),
                                RequestId = requestid
                            };
                        }
                    }


                };
            }
            catch (Exception ex)
            {
                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "Approve Appointment",
                //    Class_ = "GymSpaServices"

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new Cord][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(new { appointment, columnName }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                sr = new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.ERROR,
                    IsSuccessful = false,
                    Message = $" The appointment was not updated successfully: an error occured",
                    Status = StatusCodesExtended.Status500InternalServerError.ToString()
                };
            }

            return sr;
        }

        public async Task<ServerResponse> AvailableDates(string vendorCode)
        {
            var sr = new ServerResponse();
            string requestid = CustomFunctions.RequestID();

            try
            {
                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string sql = $"SELECT * FRPM GymSpaDate WHERE PnaVendorID =@PnaVendorID AND IsAvailable =1 AND  VendorCode =@vendorCode;";



                    var data = await con.QueryAsync<WorkingDateDTO>(sql, new { vendorCode });

                    sr = new ServerResponse
                    {

                        Data = data,
                        Code = ResponseCodes.SUCCESS,
                        Message = "Record was fetched successfully",
                        RequestId = requestid,
                        Status = StatusCodesExtended.Status200OK.ToString(),
                        IsSuccessful = true
                    };
                }
            }
            catch (Exception ex)
            {

                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "Add",
                //    Class_ = "Available Date"

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new Available dates][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(new { vendorCode }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                sr = new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.ERROR,
                    IsSuccessful = false,
                    Message = $" The appointment was not approved successfully: an error occured",
                    Status = StatusCodesExtended.Status500InternalServerError.ToString()
                };
            }
            return sr;
        }

        public async Task<ServerResponse> BookAppointment(AppointmentDTO app)
        {
            var sr = new ServerResponse();
            string requestid = CustomFunctions.RequestID();
            try
            {
                DynamicParameters paraAdd = new DynamicParameters();

                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string sql = "INSERT INTO GymSpaAppointment (AppointmentDate,Time,Duration,PnaVendorID) OUTPUT INSERTED.* VALUES " + "(@AppointmentDate,@Time,@Duration,@PnaVendorID)";
                    //paraAdd.AddDynamicParams(
                    //    new
                    //    {
                    //        AppointmentDate = app.AppointmentDate,
                    //        Time = app.Time,
                    //        Duration = app.Duration,
                    //        PnaVendorID = app.PnaVendorId,
                    //        Active = false,
                    //        Closed = false,
                    //        Approved = false,
                    //        Cancelled = false,
                    //        MemberId = app.MemberId
                    //    });

                    var trans = con.BeginTransaction();

                    var saved = await con.QueryFirstOrDefaultAsync<AppointmentDTO>(sql, app, trans);
                    if (saved != null && saved.Id > 0)
                    {
                        trans.Commit();
                        sr = new ServerResponse
                        {

                            Data = saved,
                            Code = ResponseCodes.SUCCESS,
                            IsSuccessful = true,
                            Message = $"{saved.PnaMember.FullName} appointment was  approved successfully",
                            Status = StatusCodesExtended.Status211Updated.ToString(),
                            RequestId = requestid
                        };
                    }
                    else
                    {
                        trans.Rollback();
                        sr = new ServerResponse
                        {

                            Data = null,
                            Code = ResponseCodes.UNSUCCESSFUL,
                            IsSuccessful = false,
                            Message = $" The appointment was not approved successfully and transaction rolled back",
                            Status = StatusCodesExtended.Status304NotModified.ToString()
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "Book appointment",
                //    Class_ = "GymSpaServices"

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Book new Appointment][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(app) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                sr = new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.UNSUCCESSFUL,
                    IsSuccessful = false,
                    Message = $" The appointment was not booked successfully and transaction rolled back",
                    Status = StatusCodesExtended.Status304NotModified.ToString()
                };
            }
            return sr;
        }

        public async Task<ServerResponse> CancelAppointment(long appointment, string vendorCode)
        {
            var sr = new ServerResponse();
            string requestid = CustomFunctions.RequestID();

            try
            {
                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    DynamicParameters dbPara = new DynamicParameters();
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string sql = $"UPDATE  DB_A57DC4_pnaDb_Appointment SET Cancelled=1  WHERE Id=@appointment AND  VendorCode =@vendorCode;";

                    dbPara.Add("VendorCode", vendorCode, DbType.String);
                    dbPara.Add("Id", appointment, DbType.Int64);

                    var data = await _dapper.Update<AppointmentDTO>(sql, dbPara, CommandType.Text);

                    sr = new ServerResponse
                    {

                        Data = data,
                        Code = ResponseCodes.SUCCESS,
                        Message = "Record was fetched successfully",
                        RequestId = requestid,
                        Status = StatusCodesExtended.Status200OK.ToString(),
                        IsSuccessful = true
                    };
                }
            }
            catch (Exception ex)
            {
                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "Add",
                //    Class_ = "GymSpaServices"

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Cancel appointment][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(new { appointment, vendorCode }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                sr = new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.ERROR,
                    IsSuccessful = false,
                    Message = $" The appointment was not approved successfully: an error occured",
                    Status = StatusCodesExtended.Status500InternalServerError.ToString()
                };
            }
            return sr;
        }

        public Task<ServerResponse> CheckStatusOfReceipt(ObjectNameDTO entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ServerResponse> Getallservice()
        {
            ServerResponse sr = new ServerResponse();
            string requesitid = CustomFunctions.RequestID();
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string sql = "Select * PnaVendorService ;";

                    var data = await con.QueryAsync<PnaVendorServiceDTO>(sql);
                    sr = new ServerResponse
                    {
                        Data = data,
                        Code = ResponseCodes.SUCCESS,
                        Message = "Record was fetched successfully",
                        RequestId = requesitid,
                        Status = StatusCodesExtended.Status200OK.ToString(),
                        IsSuccessful = true
                    };




                }
            }
            catch (Exception ex)
            {
                //wait Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(Getallservice) });


                await _systemUtility.JsonErrorLog($"[PNAAPI][Get All services][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(new { }) + $"| [requestId]=> {CustomFunctions.RequestID()}");

                sr = new ServerResponse
                {
                    Data = null,
                    Code = ResponseCodes.ERROR,
                    Message = "Record was not fetched successfully",
                    RequestId = requesitid,
                    Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                    IsSuccessful = false
                };
            }

            return sr;
        }

        public async Task<ServerResponse> Getallservice_by_vendor(string vendorcode)
        {
            ServerResponse sr = new ServerResponse();
            string requesitid = CustomFunctions.RequestID();
            if (!string.IsNullOrWhiteSpace(vendorcode))
            {
                try
                {
                    using (IDbConnection con = new SqlConnection(_connectionstr))
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        string sql = "Select * FROM  DB_A57DC4_pnaDb_Vendor where Vendor_code=@vendorcode;";

                        var data = await con.QueryAsync<PnaMemberDTO>(sql, new { vendorcode });
                        sr = new ServerResponse
                        {
                            Data = data,
                            Code = ResponseCodes.SUCCESS,
                            Message = "Record was fetched successfully",
                            RequestId = requesitid,
                            Status = StatusCodesExtended.Status200OK.ToString(),
                            IsSuccessful = true
                        };




                    }
                }
                catch (Exception ex)
                {
                    //await Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(Getallservice_by_vendor) });



                    await _systemUtility.JsonErrorLog($"[PNAAPI][Get service by vendor code][Response] => {ex.Message} | [request]=> " +
                                                JsonConvert.SerializeObject(new { vendorcode }) + $"| [requestId]=> {CustomFunctions.RequestID()}"); sr = new ServerResponse
                                                {
                                                    Data = null,
                                                    Code = ResponseCodes.ERROR,
                                                    Message = "Record was not fetched successfully",
                                                    RequestId = requesitid,
                                                    Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                                                    IsSuccessful = false
                                                };
                }
            }
            else
            {

                await _systemUtility.JsonErrorLog($"[PNAAPI][Get service by vendor code][Response] => {"Empty field"} | [request]=> " +
                                            JsonConvert.SerializeObject(new { vendorcode }) + $"| [requestId]=> {CustomFunctions.RequestID()}"); sr = new ServerResponse
                                            {
                                                Data = null,
                                                Code = ResponseCodes.EMPTYFIELD,
                                                Message = "Record was not fetched successfully",
                                                RequestId = requesitid,
                                                Status = StatusCodesExtended.Status204NoContent.ToString(),
                                                IsSuccessful = false
                                            };
            }


            return sr;
        }

        public async Task<ServerResponse> GetClientsStaff(string vendorCode, string staffId = "")
        {
            ServerResponse sr = new ServerResponse();
            string requesitid = CustomFunctions.RequestID();
            if (!string.IsNullOrWhiteSpace(vendorCode))
            {

                try
                {
                    using (IDbConnection con = new SqlConnection(_connectionstr))
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        string sql = string.Empty;
                        if (!string.IsNullOrWhiteSpace(staffId))
                        {
                            sql = "Select * FROM Staff where Vendor_code=@vendorCode and StaffID=@staffId;";

                        }
                        else
                        {
                            sql = "Select * Staff where Vendor_code=@vendorCode ;";
                        }

                        var data = await con.QueryAsync<StaffDTO>(sql, new { vendorCode, staffId });
                        sr = new ServerResponse
                        {
                            Data = data,
                            Code = ResponseCodes.SUCCESS,
                            Message = "Record was fetched successfully",
                            RequestId = requesitid,
                            Status = StatusCodesExtended.Status200OK.ToString(),
                            IsSuccessful = true
                        };

                    }
                }
                catch (Exception ex)
                {
                    // var obj = await Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(GetClientsStaff) });
                    await _systemUtility.JsonErrorLog($"[PNAAPI][Get client staff][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(new { vendorCode, staffId }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                    sr = new ServerResponse
                    {
                        Data = null,
                        Code = ResponseCodes.ERROR,
                        Message = "Record was not fetched successfully",
                        RequestId = requesitid,
                        Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                        IsSuccessful = false
                    };
                }
            }
            else
            {


                sr = new ServerResponse
                {
                    Data = null,
                    Code = ResponseCodes.EMPTYFIELD,
                    Message = "Please provide the vendor code",
                    RequestId = requesitid,
                    Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                    IsSuccessful = false
                };
            }


            return sr;
        }

        public async Task<ServerResponse> GetDiscountLevel(long productId)
        {
            ServerResponse sr = new ServerResponse();
            string requesitid = CustomFunctions.RequestID();
            if (productId > 0)
            {

                try
                {
                    using (IDbConnection con = new SqlConnection(_connectionstr))
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        string sql = "Select * FROM  DiscountLevel where Id=@productId ;";


                        var data = await con.QueryAsync<DiscountLevelDTO>(sql, new { productId });
                        sr = new ServerResponse
                        {
                            Data = data,
                            Code = ResponseCodes.SUCCESS,
                            Message = "Record was fetched successfully",
                            RequestId = requesitid,
                            Status = StatusCodesExtended.Status200OK.ToString(),
                            IsSuccessful = true
                        };

                    }
                }
                catch (Exception ex)
                {
                    //await Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(GetDiscountLevel) });



                    await _systemUtility.JsonErrorLog($"[PNAAPI][Get Discount level][Response] => {ex.Message} | [request]=> " +
                                      JsonConvert.SerializeObject(new { productId }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                    sr = new ServerResponse
                    {
                        Data = null,
                        Code = ResponseCodes.ERROR,
                        Message = "Record was fetched successfully",
                        RequestId = requesitid,
                        Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                        IsSuccessful = false
                    };
                }
            }
            else
            {
                sr = new ServerResponse
                {
                    Data = null,
                    Code = ResponseCodes.EMPTYFIELD,
                    Message = "Please provide the Discount level id",
                    RequestId = requesitid,
                    Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                    IsSuccessful = false
                };
            }


            return sr;
        }

        public async Task<ServerResponse> GetServiceProviders()
        {
            ServerResponse sr = new ServerResponse();
            string requesitid = CustomFunctions.RequestID();
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string sql = "Select * FROM DB_A57DC4_pnaDb_Vendor;";
                    var data = await con.QueryAsync<PnaMemberDTO>(sql);


                    sr = new ServerResponse
                    {
                        Data = data,
                        Code = ResponseCodes.SUCCESS,
                        Message = "Record was fetched successfully",
                        RequestId = requesitid,
                        Status = StatusCodesExtended.Status200OK.ToString(),
                        IsSuccessful = true
                    };



                }
            }
            catch (Exception ex)
            {
                // await Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(GetServiceProviders) });


                var obj = await Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(GetClientsStaff) });
                await _systemUtility.JsonErrorLog($"[PNAAPI][Get Service providers][Response] => {ex.Message} | [request]=> " +
                                        JsonConvert.SerializeObject(new { }) + $"| [requestId]=> {CustomFunctions.RequestID()}");

                sr = new ServerResponse
                {
                    Data = null,
                    Code = ResponseCodes.ERROR,
                    Message = "Record was not fetched successfully",
                    RequestId = requesitid,
                    Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                    IsSuccessful = false
                };
            }

            return sr;
        }

        public Task<ServerResponse> PaymentForService(OderRequest oderRequest, List<ProductDTO> products)
        {
            throw new NotImplementedException();
        }

        public async Task<ServerResponse> PreviewSalesrecordsAndCommission(DateTime date, string vendorCode)
        {
            ServerResponse sr = new ServerResponse();
            string requesitid = CustomFunctions.RequestID();
            if (!string.IsNullOrWhiteSpace(vendorCode) && date != default(DateTime))
            {

                try
                {
                    using (IDbConnection con = new SqlConnection(_connectionstr))
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        string sql = "Select * FROM GymSpaSales where DateSoled=@dateSoled and PnaVendorID=@PnaVendorID;";

                        using var trans = con.BeginTransaction();
                        var data = await con.QueryAsync<DiscountLevelDTO>(sql, new { date, vendorCode });
                        sr = new ServerResponse
                        {
                            Data = data,
                            Code = ResponseCodes.SUCCESS,
                            Message = "Record was fetched successfully",
                            RequestId = requesitid,
                            Status = StatusCodesExtended.Status200OK.ToString(),
                            IsSuccessful = true
                        };

                    }
                }
                catch (Exception ex)
                {
                    // await Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(GetDiscountLevel) });



                    await _systemUtility.JsonErrorLog($"[PNAAPI][Get Discount level][Response] => {ex.Message} | [request]=> " +
                                      JsonConvert.SerializeObject(new { date, vendorCode }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                    sr = new ServerResponse
                    {
                        Data = null,
                        Code = ResponseCodes.ERROR,
                        Message = "Record was not fetched successfully",
                        RequestId = requesitid,
                        Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                        IsSuccessful = false
                    };
                }
            }
            else
            {
                sr = new ServerResponse
                {
                    Data = null,
                    Code = ResponseCodes.EMPTYFIELD,
                    Message = "Please provide the Discount level id",
                    RequestId = requesitid,
                    Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                    IsSuccessful = false
                };
            }


            return sr;
        }

        public async Task<ServerResponse> PrintReceipt(PrintReceiptRequest printOrder)
        {
            try
            {


                if (printOrder != null && !string.IsNullOrWhiteSpace(printOrder.MembershipId) && printOrder.vendorId > 0 && printOrder.OrderId > 0)
                {

                    var data = await _getEntities.GetOrderd(printOrder.OrderId, (int)printOrder.vendorId, printOrder.MembershipId);

                    var dat = data.ToList();
                    var TotalAmount = data.Sum(p => (p.TotalCost));
                    var items = data.Select(p => p.OrderDetails).ToList();

                    List<PrintOrderReturnedDTO> printOrderReturnedDTO = new List<PrintOrderReturnedDTO>();
                    dat.ForEach(
                        g =>
                        {

                            items.ForEach(p =>
           {

               List<PrintOrderCollectionDTO> orderCollectionDTOs = new List<PrintOrderCollectionDTO>();
               var pto = p.ToList();
               pto.ForEach(k =>
               {
                   orderCollectionDTOs.Add(new PrintOrderCollectionDTO
                   {
                       Amount = k.Item_price,
                       ItemName = k.Item,
                       Quantity = k.Qunatity
                   });


               });
               printOrderReturnedDTO.Add(new PrintOrderReturnedDTO
               {

                   ListItemPriceAndQuantity = orderCollectionDTOs,
                   OrderReference = g.OrderCode,
                   ServiceCategory = g.Service_category,
                   Transactiondate = g.Order_date,
                   TransactionReference = g.PaymentReference


               });


           });
                        });





                    return new ServerResponse
                    {

                        Code = ResponseCodes.SUCCESS,
                        Data = printOrderReturnedDTO,
                        IsSuccessful = true,
                        Message = "Fetched data successfully",
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status200OK.ToString()


                    };
                }
                else
                {

                    return new ServerResponse
                    {

                        Code = ResponseCodes.UNSUCCESSFUL,
                        Data = null,
                        IsSuccessful = false,
                        Message = "Receipt was not successfully Printed: orderid, vendorid and membershipId are required",
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status404NotFound.ToString()


                    };
                }

            }
            catch (Exception ex)
            {

                return new ServerResponse
                {


                    Code = ResponseCodes.ERROR,
                    Data = ex,
                    IsSuccessful = false,
                    Message = "Data was nto fetched successfully: an error occured",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status500InternalServerError.ToString()


                };
            }
        }

        public Task<ServerResponse> Send_emailnotification_pnaadmin(Orderto_pna_admin_emailreq orderto_Pna_Admin_Emailreq)
        {
            throw new NotImplementedException();
        }

        public async Task<ServerResponse> SendEmailmailnotification(Orderto_pna_admin_emailreqNew orderto_Pna_Admin_Emailreq)
        {

            string requestId = CustomFunctions.RequestID();
            ServerResponse sr = new ServerResponse();

            requestId = Helper.GenerateUniqueId(7);
            string mailto = string.Empty;
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                //get email template
                var subject = $"New {orderto_Pna_Admin_Emailreq.member_fnmae} Order {orderto_Pna_Admin_Emailreq.orderid} - has been requested.";
                string path1 = @"Resources\EmailTemplate\Ordertovendor_notification.html";
                string fullPath;
                fullPath = Path.GetFullPath(path1);
                var sendToType = orderto_Pna_Admin_Emailreq.sendEmailType;
                StreamReader str = new StreamReader(fullPath);
                string MailText = str.ReadToEnd(); str.Close();
                if (sendToType == SendEmailType.Admin)
                {
                    mailto = "services@playnetworkafricaservices.com";
                }
                else if (sendToType == SendEmailType.Vendor)
                {
                    mailto = orderto_Pna_Admin_Emailreq.vendoremail;
                }
                else if (sendToType == SendEmailType.Member)
                {

                }
                else if (sendToType == SendEmailType.Nonmember)
                {

                }


                MailText = MailText.Replace("[orderid]", orderto_Pna_Admin_Emailreq.orderid);
                MailText = MailText.Replace("[vendorname]", orderto_Pna_Admin_Emailreq.vendorname);
                MailText = MailText.Replace("[member_fnmae]", orderto_Pna_Admin_Emailreq.member_fnmae);
                MailText = MailText.Replace("[member_phone]", orderto_Pna_Admin_Emailreq.member_phone);
                MailText = MailText.Replace("[item]", orderto_Pna_Admin_Emailreq.item);
                MailText = MailText.Replace("[orderdate]", orderto_Pna_Admin_Emailreq.orderdate);
                MailText = MailText.Replace("[service_category]", orderto_Pna_Admin_Emailreq.service_category);
                MailText = MailText.Replace("[totalpay]", orderto_Pna_Admin_Emailreq.totalpay.ToString("N", CultureInfo.CurrentCulture));
                var SendEmailApi = new SendEmailApiReq()
                {
                    htmlBody = MailText,
                    MailSubject = subject,
                    MailTo = mailto
                };

                var res = await _notificationServices.sendmail(SendEmailApi);
                if (res == "Ok")
                {
                    sr = new ServerResponse
                    {
                        Code = ResponseCodes.SUCCESS,
                        Data = "Ok",
                        IsSuccessful = true,
                        Message = "Email send successfully",
                        RequestId = requestId,
                        Status = "Sent"


                    };
                }

            }
            catch (Exception ex)
            {
                sr = new ServerResponse
                {
                    Code = ResponseCodes.UNSUCCESSFUL,
                    Data = ex,
                    IsSuccessful = false,
                    Message = "Email was not send successfully",
                    RequestId = requestId,
                    Status = StatusCodesExtended.Status500InternalServerError.ToString()


                };
                await _systemUtility.JsonErrorLog($"[PNAAPI][Send_emailnotification_vendor][Response] => {ex.Message} | [request]=> " +
                                               JsonConvert.SerializeObject(orderto_Pna_Admin_Emailreq) + $"| [requestId]=> {requestId}");
            }
            return sr;
        }

        public async Task<ServerResponse> SetWorkingDaysandTime(WorkingDateDTO[] dates)
        {
            ServerResponse sr = new ServerResponse();
            string requesitid = CustomFunctions.RequestID();
            if (dates != null && dates.Count() > 0)
            {
                try
                {
                    using (IDbConnection con = new SqlConnection(_connectionstr))
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }


                        var sql = "[dbo].[SPGymSpaDates]";

                        using (var trans = con.BeginTransaction())
                        {
                            var data = await con.QueryAsync<WorkingDateDTO>(sql, dates);


                            sr = new ServerResponse
                            {
                                Data = data,
                                Code = ResponseCodes.SUCCESS,
                                Message = "Record was fetched successfully",
                                RequestId = requesitid,
                                Status = StatusCodesExtended.Status200OK.ToString(),
                                IsSuccessful = true
                            };


                        }



                    }
                }
                catch (Exception ex)
                {
                    //  await Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(SetWorkingDaysandTime) });


                    await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new Cord][Response] => {ex.Message} | [request]=> " +
                                          JsonConvert.SerializeObject(new { dates }) + $"| [requestId]=> {CustomFunctions.RequestID()}");


                    sr = new ServerResponse
                    {
                        Data = null,
                        Code = ResponseCodes.ERROR,
                        Message = "Record was not fetched successfully",
                        RequestId = requesitid,
                        Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                        IsSuccessful = false
                    };
                }
            }
            else
            {

                sr = new ServerResponse
                {
                    Data = null,
                    Code = ResponseCodes.EMPTYFIELD,
                    Message = "Record was not fetched successfully",
                    RequestId = requesitid,
                    Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                    IsSuccessful = true
                };
            }
            return sr;


        }

        public async Task<ServerResponse> TotalCost(List<OrderDTO> entity)
        {
            ServerResponse sr = new ServerResponse();
            if (entity != null && entity.Count > 0)
            {


                try
                {
                    var data = entity.Sum(p => (p.OrderDetails.Sum(x => (x.Item_price - x.Discounted_amount))));
                    sr = new ServerResponse
                    {
                        Code = ResponseCodes.SUCCESS,
                        IsSuccessful = true,
                        RequestId = CustomFunctions.RequestID(),
                        Data = data,
                        Status = StatusCodesExtended.Status200OK.ToString()
                    };
                }
                catch (Exception ex)
                {
                    // await Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(TotalCost) });


                    await _systemUtility.JsonErrorLog($"[PNAAPI][get total cost of order][Response] => {ex.Message} | [request]=> " +
                                          JsonConvert.SerializeObject(new { entity }) + $"| [requestId]=> {CustomFunctions.RequestID()}");

                    sr = new ServerResponse
                    {
                        Data = null,
                        Code = ResponseCodes.ERROR,
                        Message = "Record was not fetched successfully",
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                        IsSuccessful = false
                    };
                }

            }
            else
            {

                sr = new ServerResponse
                {
                    Data = null,
                    Code = ResponseCodes.EMPTYFIELD,
                    Message = "Record was not fetched successfully",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                    IsSuccessful = true
                };
            }
            return sr;
        }

        public Task<ServerResponse> Update(object entity, long typeId)
        {
            throw new NotImplementedException();
        }
        [NotImplemented("Not yet Implemented")]
        public Task<ServerResponse> UplaodStaffNames(IFormFile file)
        {
            throw new NotImplementedException();
        }

        [NotImplemented("Not ye implemented")]
        Task<ServerResponse> UploadImage(IFormFile file, string fileName, UploadType uploadType, long productId = 0, int vendorId = 0)
        {
            throw new NotImplementedException();
        }
        public async Task<ServerResponse> UploadFile(IFormFile file, string fileName, UploadType uploadType)
        {
            ServerResponse sr = new ServerResponse();

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                try
                {
                    string path = string.Empty;

                    if (file == null || file.Length == 0)
                    {
                        sr = new ServerResponse { Data = null, Message = "file not selected", Code = ResponseCodes.NOT_PROCESSED, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status410InvalidObject.ToString() };

                    }
                    else
                    {
                        var fileExtension = Path.GetExtension(file.FileName);

                        if (uploadType == UploadType.Document)
                        {
                            path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", "Documents", "Products", "Images", fileName);
                        }

                        else if (uploadType == UploadType.Product)
                        {
                            path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", "Products", "Products", "Images", fileName);
                        }




                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        sr = new ServerResponse { Data = path, Message = "file processed", Code = ResponseCodes.SUCCESS, RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status200OK.ToString() };

                    }
                }
                catch (Exception ex)
                {
                    // await Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(TotalCost) });

                    await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new Cord][Response] => {ex.Message} | [request]=> " +
                                                          JsonConvert.SerializeObject(new { file, fileName, uploadType }) + $"| [requestId]=> {CustomFunctions.RequestID()}");

                    sr = new ServerResponse
                    {
                        Data = null,
                        Code = ResponseCodes.ERROR,
                        Message = "Record was not fetched successfully",
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                        IsSuccessful = false
                    };
                }
            }

            else
            {

                sr = new ServerResponse
                {
                    Data = null,
                    Code = ResponseCodes.EMPTYFIELD,
                    Message = "Record was not fetched successfully",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                    IsSuccessful = false
                };
            }
            return sr;
        }


        public async Task<ServerResponse> GetMemberOrder(int vendorId, long orderId, string memberId, DateTime orderDate)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string sql = "Select * From GymSpaOrder where Id=@orderId and MembershipId=@memberId and Order_date=@orderDate and VendorID=@vendorId;";
                    var trans = con.BeginTransaction();
                    var data = await con.QueryFirstAsync<OrderDTO>(sql, new { orderId, memberId, orderDate, vendorId });
                    return new ServerResponse
                    {

                        Data = data,
                        Code = ResponseCodes.SUCCESS,
                        Message = "Record was fetchecd successfully",
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status200OK.ToString(),
                        IsSuccessful = true

                    };

                }
            }
            catch (Exception ex)
            {
                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "Add",
                //    Class_ = "GymSpaServices"

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new Cord][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(new { vendorId, orderId, memberId, orderDate }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                return new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.ERROR,
                    Message = "Record was not fetcj=hed successfully",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status200OK.ToString(),
                    IsSuccessful = false

                };
            }

        }
        public async Task<ServerResponse> GetMemberOrderDetail(long orderId, string memberId, DateTime orderDate)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string sql = "Select * From GymSpaOrderDetail where OrderId=@orderId and MembershipId=@memberId and Order_date=@orderDate;";
                    var trans = con.BeginTransaction();
                    var data = await con.QueryAsync<OrderDetailDTO>(sql, new { orderId, memberId, orderDate });
                    return new ServerResponse
                    {

                        Data = data,
                        Code = ResponseCodes.SUCCESS,
                        Message = "Record was fetchecd successfully",
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status200OK.ToString(),
                        IsSuccessful = true

                    };

                }
            }
            catch (Exception ex)
            {
                //    await Add(new ErrorLogDbDTO
                //    {

                //        DateCreated = DateTime.Now,
                //        Exception = ex.ToString(),
                //        Method = "Add",
                //        Class_ = "GymSpaServices"

                //    });
                await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new Cord][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(new { orderId, memberId, orderDate }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                return new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.ERROR,
                    Message = "Record was not fetcj=hed successfully",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status200OK.ToString(),
                    IsSuccessful = true

                };
            }

        }

        public async Task<ServerResponse> GetMemberOrderAmount(IEnumerable<OrderDetailDTO> orderDetailCollection)
        {
            var sr = new ServerResponse();
            try
            {
                var data = orderDetailCollection.Sum(p => (p.Item_price - (p.DiscountedInPercent * p.Item_price)));

                sr = new ServerResponse
                {
                    Data = data,
                    Code = ResponseCodes.SUCCESS,
                    Message = "Record was fetched successfully",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                    IsSuccessful = true
                };

            }
            catch (Exception ex)
            {
                // await Add(new ErrorLogDbDTO { Class_ = "GymSpaServices", DateCreated = DateTime.Now, Exception = ex.ToString(), Method = nameof(TotalCost) });


                await _systemUtility.JsonErrorLog($"[PNAAPI][get Member Order Amount][Response] => {ex.Message} | [request]=> " +
                                      JsonConvert.SerializeObject(new { orderDetailCollection }) + $"| [requestId]=> {CustomFunctions.RequestID()}");

                sr = new ServerResponse
                {
                    Data = null,
                    Code = ResponseCodes.ERROR,
                    Message = "Record was not  fetched successfully",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                    IsSuccessful = false
                };
            }
            return sr;

        }
        public async Task<ServerResponse> View(object entity, long Id, long errorLogDbId = 0, long orderId = 0, long categoryId = 0, int vendorId = 0, string vendorCode = "", string staffId = "", DateTime? aloteddate = null, long productId = 0, string memberId = null)
        {
            try
            {

                if (Id > 0)
                {
                    dynamic data = null;
                    string sql = string.Empty;
                    string requestid = CustomFunctions.RequestID();
                    if (entity.GetType() == typeof(AppointmentDTO))
                    {
                        if (!string.IsNullOrWhiteSpace(vendorCode))
                        {
                            data = await _getEntities.GetAppointmentd(Id, vendorId);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }


                    }
                    else if (entity.GetType() == typeof(WorkingDateDTO))
                    {
                        if (!string.IsNullOrWhiteSpace(vendorCode))
                        {
                            data = await _getEntities.GetWorkingDated(Id, vendorCode);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }


                    }
                    else if (entity.GetType() == typeof(DurationDTO))
                    {
                        if ((vendorId) > 0)
                        {
                            data = await _getEntities.GetDuration(Id, vendorId, (DateTime)aloteddate);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }


                    }
                    else if (entity.GetType() == typeof(DiscountLevelDTO))
                    {
                        if (!string.IsNullOrWhiteSpace(vendorCode))
                        {
                            data = await _getEntities.GetDiscountLeveld(Id, vendorId);

                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }


                    }
                    else if (entity.GetType() == typeof(OrderDetailDTO))
                    {
                        if (orderId > 0 && vendorId > 0)
                        {
                            data = await _getEntities.GetOrderDetaild(Id, orderId, vendorId);


                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }

                    }

                    else if (entity.GetType() == typeof(OrderDTO))
                    {
                        if (vendorId > 0)
                        {
                            data = await _getEntities.GetOrderd(Id, vendorId);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }


                    }

                    else if (entity.GetType() == typeof(ProductDTO))
                    {


                        if (categoryId > 0 && !string.IsNullOrWhiteSpace(vendorCode))
                        {
                            data = await _getEntities.GetProductd(Id, categoryId, vendorCode);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }

                    }

                    else if (entity.GetType() == typeof(ProductCategoryDTO))
                    {



                        if (orderId > 0 && vendorId > 0)
                        {
                            data = _getEntities.GetProductCategoryd(Id, vendorId);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }

                    }

                    else if (entity.GetType() == typeof(SalesDTO))
                    {


                        if (orderId > 0 && vendorId > 0)
                        {
                            data = await _getEntities.GetSale(Id, orderId, vendorId);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }


                    }


                    else if (entity.GetType() == typeof(StaffDTO))
                    {

                        if (vendorId > 0)
                        {
                            data = await _getEntities.GetStaffd(staffId, vendorCode);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }


                    }


                    else if (entity.GetType() == typeof(ErrorLogDbDTO))
                    {

                        if (vendorId > 0)
                        {
                            data = await _getEntities.GetErrorLogDbd(Id, vendorId);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }

                    }
                    else if (entity.GetType() == typeof(ErrorLogSolutionDTO))
                    {

                        if (errorLogDbId > 0)
                        {
                            data = await _getEntities.GetErrorLogSolutiond(Id, errorLogDbId);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }


                    }
                    else if (entity.GetType() == typeof(DutyRoasterDTO))
                    {

                        if (vendorId > 0)
                        {

                            data = await _getEntities.GetDutyRoaster(Id, vendorId, staffId);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }


                    }
                    else if (entity.GetType() == typeof(EventDTO))
                    {

                        if (vendorId > 0)
                        {

                            data = await _getEntities.GetDutyRoaster(Id, vendorId, staffId);
                        }
                        else
                        {
                            data = new ServerResponse
                            {
                                Data = null,
                                Code = ResponseCodes.EMPTYFIELD,
                                Message = "Empty filed is indicated",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                                IsSuccessful = true
                            };
                        }


                    }
                    if (data != null && data.Id > 0)
                    {

                        if (data.Id > 0)
                        {
                            return new ServerResponse
                            {

                                Data = data,
                                Code = ResponseCodes.SUCCESS,
                                Message = "Record was created successfully",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status200OK.ToString(),
                                IsSuccessful = true

                            };
                        }
                        else
                        {
                            return new ServerResponse
                            {

                                Data = data,
                                Code = ResponseCodes.UNSUCCESSFUL,
                                Message = "Record was not created successfully",
                                RequestId = requestid,
                                Status = StatusCodesExtended.Status411NotCreated.ToString(),
                                IsSuccessful = false

                            };
                        }
                    }
                    else
                    {
                        return new ServerResponse
                        {

                            Data = data,
                            Code = ResponseCodes.UNSUCCESSFUL,
                            Message = "Record was not created successfully",
                            RequestId = requestid,
                            Status = StatusCodesExtended.Status411NotCreated.ToString(),
                            IsSuccessful = false

                        };
                    }


                }
                else
                {
                    return new ServerResponse
                    {
                        Data = null,
                        Code = ResponseCodes.EMPTYFIELD,
                        Message = "Empty filed is indicated",
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status410InvalidObject.ToString(),
                        IsSuccessful = false
                    };
                }

            }
            catch (Exception ex)
            {
                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "Add",
                //    Class_ = "GymSpaServices"

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new Cord][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(entity) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                return new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.ERROR,
                    Message = "Record was not fetched successfully",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status200OK.ToString(),
                    IsSuccessful = false

                };
            }
        }

        [NotImplemented("This method has not been Implemented yet: to be considered!")]
        public Task<ServerResponse> ViewAll(Expression<Func<Type, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<ServerResponse> ViewAll(object entity)
        {
            try
            {

                dynamic data = null;
                string sql = string.Empty;
                string requestid = CustomFunctions.RequestID();
                if (entity.GetType() == typeof(AppointmentDTO))
                {
                    data = await _getEntities.GetAppointmentds();
                }
                else if (entity.GetType() == typeof(WorkingDateDTO))
                {
                    data = await _getEntities.GetWorkingDateds();
                }
                else if (entity.GetType() == typeof(DiscountLevelDTO))
                {
                    data = await _getEntities.GetDiscountLevelds();
                }
                else if (entity.GetType() == typeof(OrderDetailDTO))
                {
                    data = await _getEntities.GetOrderDetailds();
                }

                else if (entity.GetType() == typeof(OrderDTO))
                {
                    data = await _getEntities.GetOrderds();
                }

                else if (entity.GetType() == typeof(ProductDTO))
                {

                    data = await _getEntities.GetProductds();
                }

                else if (entity.GetType() == typeof(ProductCategoryDTO))
                {


                    data = _getEntities.GetProductCategoryds();
                }

                else if (entity.GetType() == typeof(SalesDTO))
                {



                    data = await _getEntities.GetSales();
                }


                else if (entity.GetType() == typeof(StaffDTO))
                {



                    data = await _getEntities.GetStaffds();
                }


                else if (entity.GetType() == typeof(ErrorLogDbDTO))
                {
                    data = await _getEntities.GetErrorLogDbds();
                }
                else if (entity.GetType() == typeof(ErrorLogSolutionDTO))
                {


                    data = await _getEntities.GetErrorLogSolutionds();

                }

                if (data != null && data.Count > 0)
                {

                    if (data.Id > 0)
                    {
                        return new ServerResponse
                        {

                            Data = data,
                            Code = ResponseCodes.SUCCESS,
                            Message = "Record was fetched successfully",
                            RequestId = requestid,
                            Status = StatusCodesExtended.Status200OK.ToString(),
                            IsSuccessful = true

                        };
                    }
                    else
                    {
                        return new ServerResponse
                        {

                            Data = data,
                            Code = ResponseCodes.UNSUCCESSFUL,
                            Message = "Record was not fetched successfully",
                            RequestId = requestid,
                            Status = StatusCodesExtended.Status404NotFound.ToString(),
                            IsSuccessful = false

                        };
                    }
                }
                else
                {
                    return new ServerResponse
                    {

                        Data = data,
                        Code = ResponseCodes.SUCCESS,
                        Message = "No record was found",
                        RequestId = requestid,
                        Status = StatusCodesExtended.Status200OK.ToString(),
                        IsSuccessful = true

                    };
                }


            }
            catch (Exception ex)
            {
                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "Add",
                //    Class_ = "GymSpaServices"

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Adding new Cord][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(entity) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                return new ServerResponse
                {

                    Data = ex,
                    Code = ResponseCodes.ERROR,
                    Message = "An error occured",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status500InternalServerError.ToString(),
                    IsSuccessful = false

                };
            }
        }

        [NotImplemented("Removed from implementation , but can be look at for consideration")]
        Task<ServerResponse> IGymSpaService<ServerResponse>.UploadImage(IFormFile file, string fileName, UploadType uploadType, long productId, int vendorId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServerResponse> AvailableStaffs(int vendorId)
        {
            var sql = "Select s.* , v.* from Staff s , PnaVendor v Where s.VendorId=@vendorId and s.VendorId=v.Id and IsAvailable=1 and IsActivated=1;";
            var sr = new ServerResponse();
            try
            {


                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<StaffDTO, PnaVendorDTO, StaffDTO>(sql,
                        (s, v) =>
                        {
                            s.PnaVendor = v;
                            return s;
                        }
                        ,
                        new { vendorId }, splitOn: "Id");
                    sr = new ServerResponse { Code = ResponseCodes.SUCCESS, Data = data, IsSuccessful = true, Message = "Record was created successfullt", RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status201Created.ToString() };
                }
            }
            catch (Exception ex)
            {

                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "AvailableStaffs",
                //    Class_ = "GymSpaServices"

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Get available staffs per vendor][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(new { vendorId }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                return new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.ERROR,
                    Message = "Record was not fetched successfully",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status200OK.ToString(),
                    IsSuccessful = false

                };
            }
            return sr;
        }



        async Task<ServerResponse> IGymSpaService<ServerResponse>.TotalCostOfAnAppointment(int vendorId, int memberId, DateTime appointmentdate)
        {

            var sql = "Select * from GymSpaAppointment Where VendorId=@vendorId and Appointmentdate=@appointmentdate and MemberId=@memberId;";
            var sr = new ServerResponse();
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<AppointmentDTO>(sql, new { vendorId, memberId, appointmentdate });
                    var cost = data.Sum(p => p.Cost);
                    var dcount = data.Sum(p => p.Discount);
                    var realCost = cost - dcount;
                    var dta = $"{cost}:{dcount}:{realCost}";
                    sr = new ServerResponse { Code = ResponseCodes.SUCCESS, Data = dta, IsSuccessful = true, Message = "Record was created successfullt", RequestId = CustomFunctions.RequestID(), Status = StatusCodesExtended.Status201Created.ToString() };
                }
            }
            catch (Exception ex)
            {

                //await Add(new ErrorLogDbDTO
                //{

                //    DateCreated = DateTime.Now,
                //    Exception = ex.ToString(),
                //    Method = "TotalCostOfAnAppointment",
                //    Class_ = "GymSpaServices"

                //});
                await _systemUtility.JsonErrorLog($"[PNAAPI][Get total appointment Cost][Response] => {ex.Message} | [request]=> " +
                                            JsonConvert.SerializeObject(new { vendorId, memberId, appointmentdate }) + $"| [requestId]=> {CustomFunctions.RequestID()}");
                return new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.ERROR,
                    Message = "Record was not fetched successfully",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status200OK.ToString(),
                    IsSuccessful = false

                };
            }
            return sr;
        }

        public async Task<ServerResponse> FileExists(object type)
        {

            if ((type.GetType() == typeof(ProductCategoryDTO)) == true)
            {
                string sql = "SELECT * FROM DB_A57DC4_pnaDb_GymSpaProductCategory WHERE Name=@Name;";
                using (IDbConnection con = new SqlConnection(_connectionstr))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var entity = (ProductCategoryDTO)type;
                    var data = await con.QuerySingleOrDefaultAsync(sql, new { Name = entity.Name });
                    return new ServerResponse
                    {

                        Data = data,
                        Code = ResponseCodes.SUCCESS,
                        IsSuccessful = true,
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status200OK.ToString(),
                        Message = "Name already exists"


                    };

                }
            }
            else
            {
                return new ServerResponse
                {

                    Data = null,
                    Code = ResponseCodes.UNSUCCESSFUL,
                    IsSuccessful = false,
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status413Unsessful.ToString()


                };
            }


        }

        public async Task<ServerResponse> CreateOrder(OrderDTORequest order, object products)
        {
            bool emailSent = false;
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            var logmonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);

            string Generatepassword = CustomFunctions.GeneratePassword();
            string ReferralCode = CustomFunctions.ReferalCode();
            string OrderID = CustomFunctions.OrderId();

            string WalletId = Helper.GenerateUniqueId(6);
            Random Rnd = new Random();
            string ActivationCode = (Rnd.Next(100000, 999999)).ToString();
            try
            {
                if (string.IsNullOrWhiteSpace(order.Demand_Type) || string.IsNullOrWhiteSpace(order.MembershipID) || string.IsNullOrWhiteSpace(order.Service_category)
                    || string.IsNullOrWhiteSpace(order.Table_type) || string.IsNullOrWhiteSpace(order.Vendor_code) || string.IsNullOrWhiteSpace(order.ItemCode))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Most Fields are required");
                }
                else if (!order.Demand_Type.All(c => char.IsLetter(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "demand_type must be in Letter");
                }
                else if (!order.MembershipID.All(c => char.IsLetterOrDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "MembershipID must be in Letter and digit");
                }
                else if (!order.Vendor_code.All(c => char.IsLetterOrDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Vendor_code must be in Letter and digit");
                }
                else if (!order.ItemCode.All(c => char.IsLetterOrDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "item_code must be in Letter and digit");
                }
                else
                {
                    string selectQuerylogon = string.Empty;
                    string selectQuerylogon3 = string.Empty;
                    // check for vendor and member
                    IEnumerable<PnaMember> memberresults;
                    IEnumerable<PnaVendor> vendorresults;
                    IEnumerable<NightLifeProducts> NightLifeProductsresults = null;
                    IEnumerable<ProductDTO> productDTOs = null; ;
                    if (products.GetType() == typeof(NightLifeProducts))
                    {
                        // check for item 
                        selectQuerylogon3 = @"SELECT * FROM [NightLifeProducts] where item_code=@item_code";

                    }
                    else if (products.GetType() == typeof(ProductDTO))
                    {
                        // check for item 
                        selectQuerylogon3 = @"SELECT * FROM [Product] where item_code=@item_code";
                    }
                    else if (products.GetType() == typeof(EventDTO))
                    {
                        // check for item 
                        selectQuerylogon3 = @"SELECT * FROM [Event] where ItemCode=@item_code";
                    }
                    using (IDbConnection connlogon = new SqlConnection())
                    {
                        selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where MembershipID=@MembershipID";

                        memberresults = await connlogon.QueryAsync<PnaMember>(selectQuerylogon, new
                        {
                            order.MembershipID
                        });
                        int rowlogon = memberresults.Count();
                        if (rowlogon >= 1)
                        {
                            // check for vendor
                            string selectQuerylogon2 = @"SELECT * FROM [DB_A57DC4_pnaDb_Vendor] where Vendor_code=@Vendor_code";

                            vendorresults = await connlogon.QueryAsync<PnaVendor>(selectQuerylogon2, new
                            {
                                order.Vendor_code
                            });
                            int rowlogon2 = vendorresults.Count();
                            if (rowlogon2 >= 1)
                            {
                                string srrvcate = string.Empty;
                                int rowlogon3 = 0;

                                if (products.GetType() == typeof(NightLifeProducts))
                                {
                                    // check for item 
                                    selectQuerylogon3 = @"SELECT * FROM [NightLifeProducts] where item_code=@item_code";
                                    NightLifeProductsresults = await connlogon.QueryAsync<NightLifeProducts>(selectQuerylogon3, new
                                    {
                                        order.ItemCode
                                    });
                                    rowlogon3 = NightLifeProductsresults != null ? NightLifeProductsresults.Count() : 0;
                                }
                                else if (products.GetType() == typeof(ProductDTO))
                                {
                                    // check for item 
                                    selectQuerylogon3 = @"SELECT * FROM [Product] where item_code=@item_code";
                                    productDTOs = await connlogon.QueryAsync<ProductDTO>(selectQuerylogon3, new
                                    {
                                        order.ItemCode
                                    });
                                    rowlogon3 = productDTOs != null ? productDTOs.Count() : 0;
                                }
                                long OrderInsert = 0;
                                if (rowlogon3 >= 1)
                                {
                                    // spool their info
                                    foreach (var memberinfo in memberresults)
                                    {
                                        foreach (var vendorinfo in vendorresults)
                                        {

                                            if (products.GetType() == typeof(NightLifeProducts) == true)
                                            {
                                                foreach (var iteminfo in NightLifeProductsresults)
                                                {
                                                    //get discounted amount
                                                    var getDiscounted_price = iteminfo.item_price - (iteminfo.item_price * iteminfo.item_discount / 100);

                                                    var setproduct_itemto_order = new NightLifeOrders()
                                                    {
                                                        demand_type = order.Demand_Type,
                                                        item_price = iteminfo.item_price,
                                                        discounted_amount = getDiscounted_price,
                                                        item = iteminfo.item,
                                                        OrderId = OrderID,
                                                        MembershipID = order.MembershipID,
                                                        number_of_persons = order.Number_of_persons,
                                                        IsItem_paid = false,
                                                        IsVendorCredited = false,
                                                        paymentMethod = "Wallet",
                                                        reservation_date = order.Reservation_date,
                                                        service_category = order.Service_category,
                                                        service_type = order.Service_type,
                                                        table_type = order.Table_type,
                                                        total_payable_amount = getDiscounted_price,
                                                        order_date = logdate,
                                                        Vendor_code = order.Vendor_code
                                                    };
                                                    var setDTO = new NightLife_OrdersDTO()
                                                    {
                                                        ItemName = iteminfo.item,
                                                        ItemPrice = iteminfo.item_price,
                                                        ItemDiscount = iteminfo.item_discount.ToString() + "%",
                                                        Item_DiscountedPrice = getDiscounted_price,
                                                        OrderReference = OrderID,
                                                        Total_payable = getDiscounted_price
                                                    };
                                                    OrderInsert = connlogon.Insert(setproduct_itemto_order);
                                                    srrvcate = order.Service_category;
                                                    //check if record inserted well
                                                    if (OrderInsert >= 1)
                                                    {
                                                        // send Order reciept to email member
                                                        var subject = $"Your {vendorinfo.companyName} Order {OrderID} - has been confirmed.";
                                                        string path1 = @"Resources\EmailTemplate\Vending_email.html";
                                                        string fullPath;
                                                        fullPath = Path.GetFullPath(path1);
                                                        StreamReader str = new StreamReader(fullPath);
                                                        string MailText = str.ReadToEnd();
                                                        str.Close();
                                                        MailText = MailText.Replace("[fname]", memberinfo.FullName.ToUpper());
                                                        MailText = MailText.Replace("[vendorname]", vendorinfo.companyName);
                                                        MailText = MailText.Replace("[orderid]", OrderID);
                                                        MailText = MailText.Replace("[item]", iteminfo.item);
                                                        MailText = MailText.Replace("[item_price]", iteminfo.item_price.ToString("N", CultureInfo.CurrentCulture));
                                                        MailText = MailText.Replace("[discount]", iteminfo.item_discount.ToString() + "%");
                                                        MailText = MailText.Replace("[orderdate]", logdate.ToString());
                                                        MailText = MailText.Replace("[service_category]", order.Service_category);
                                                        MailText = MailText.Replace("[totalpay]", getDiscounted_price.ToString("N", CultureInfo.CurrentCulture));
                                                        var SendEmailApi = new SendEmailApiReq()
                                                        {
                                                            htmlBody = MailText,
                                                            MailSubject = subject,
                                                            MailTo = memberinfo.Email
                                                        };

                                                        await _notificationServices.sendmail(SendEmailApi);
                                                        var setvendoremail_notification = new Orderto_pna_admin_emailreqNew
                                                        {
                                                            item = iteminfo.item,
                                                            member_fnmae = memberinfo.FullName,
                                                            member_phone = memberinfo.PhoneNumbers,
                                                            orderdate = logdate.ToString(),
                                                            orderid = OrderID,
                                                            service_category = order.Service_category,
                                                            totalpay = getDiscounted_price,
                                                            vendoremail = vendorinfo.Email,
                                                            vendorname = vendorinfo.companyName,
                                                            sendEmailType = SendEmailType.Vendor
                                                        };
                                                        var setpna_adminemail_notification = new Orderto_pna_admin_emailreqNew()
                                                        {
                                                            item = iteminfo.item,
                                                            member_fnmae = memberinfo.FullName,
                                                            member_phone = memberinfo.PhoneNumbers,
                                                            orderdate = logdate.ToString(),
                                                            orderid = OrderID,
                                                            service_category = order.Service_category,
                                                            totalpay = getDiscounted_price,
                                                            vendoremail = vendorinfo.Email,
                                                            vendorname = vendorinfo.companyName,
                                                            sendEmailType = SendEmailType.Admin
                                                        };
                                                        //send mail to vendor
                                                        var vendemail = await SendEmailmailnotification(setvendoremail_notification);
                                                        if (vendemail.Code == "00")
                                                        {
                                                            //send mail to Pna Admin
                                                            var respo = await SendEmailmailnotification(setpna_adminemail_notification);

                                                            if (respo.Code == "00")
                                                            {
                                                                emailSent = true;
                                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "SUCCESS", setDTO);
                                                            }
                                                            else
                                                            {
                                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Order recorded but sending of email not successful", setDTO);
                                                            }

                                                            var syslog = new Sys_logReq()
                                                            {
                                                                channel = _appChannel.Channel1,
                                                                description = $"Member {memberinfo.FullName} made booking order_id:{OrderID} from vendor: {vendorinfo.companyName}",
                                                                UserID = memberinfo.Email,
                                                                Usertype = _appSystemType.Type7
                                                            };
                                                            var sys_log = await _systemUtility.sys_Log(syslog);
                                                        }



                                                    }
                                                    else
                                                    {

                                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"could not process Order, try again");
                                                    }

                                                }
                                            }
                                            else if (products.GetType() == typeof(ProductDTO) == true)
                                            {
                                                foreach (var iteminfo in productDTOs)
                                                {
                                                    //get discounted amount
                                                    var getDiscounted_price = iteminfo.Item_price - (iteminfo.Item_price * iteminfo.Item_Discount / 100);

                                                    var setproduct_itemto_order = new Order()
                                                    {
                                                        Demand_Type = order.Demand_Type,
                                                        Item_price = iteminfo.Item_price,
                                                        Discounted_amount = getDiscounted_price,
                                                        Item = iteminfo.Item,
                                                        OrderId = OrderID,
                                                        MembershipID = order.MembershipID,
                                                        // number_of_persons = order.Number_of_persons,
                                                        IsItem_paid = false,
                                                        IsVendorCredited = false,
                                                        PaymentMethod = "Wallet",
                                                        //reservation_date = order.Reservation_date,
                                                        Service_category = order.Service_category,
                                                        Service_type = order.Service_type,
                                                        Table_type = order.Table_type,
                                                        Total_payable_amount = getDiscounted_price,
                                                        Order_date = logdate,
                                                        Vendor_code = order.Vendor_code,
                                                    };
                                                    var setDTO = new Order_View()
                                                    {
                                                        Item = iteminfo.Item,
                                                        Item_price = iteminfo.Item_price,
                                                        Item_Discount = iteminfo.Item_Discount.ToString() + "%",

                                                        Item_DiscountedPrice = getDiscounted_price,
                                                        OrderReference = OrderID,
                                                        Total_payable = getDiscounted_price
                                                    };
                                                    OrderInsert = connlogon.Insert(setproduct_itemto_order);
                                                    srrvcate = order.Service_category;
                                                    //check if record inserted well
                                                    if (OrderInsert >= 1)
                                                    {
                                                        // send Order reciept to email member
                                                        var subject = $"Your {vendorinfo.companyName} Order {OrderID} - has been confirmed.";
                                                        string path1 = @"Resources\EmailTemplate\Vending_email.html";
                                                        string fullPath;
                                                        fullPath = Path.GetFullPath(path1);
                                                        StreamReader str = new StreamReader(fullPath);
                                                        string MailText = str.ReadToEnd();
                                                        str.Close();
                                                        MailText = MailText.Replace("[fname]", memberinfo.FullName.ToUpper());
                                                        MailText = MailText.Replace("[vendorname]", vendorinfo.companyName);
                                                        MailText = MailText.Replace("[orderid]", OrderID);
                                                        MailText = MailText.Replace("[item]", iteminfo.Item);
                                                        MailText = MailText.Replace("[item_price]", iteminfo.Item_price.ToString("N", CultureInfo.CurrentCulture));
                                                        MailText = MailText.Replace("[discount]", iteminfo.Item_Discount.ToString() + "%");
                                                        MailText = MailText.Replace("[orderdate]", logdate.ToString());
                                                        MailText = MailText.Replace("[service_category]", order.Service_category);
                                                        MailText = MailText.Replace("[totalpay]", getDiscounted_price.ToString("N", CultureInfo.CurrentCulture));
                                                        var SendEmailApi = new SendEmailApiReq()
                                                        {
                                                            htmlBody = MailText,
                                                            MailSubject = subject,
                                                            MailTo = memberinfo.Email
                                                        };

                                                        await _notificationServices.sendmail(SendEmailApi);
                                                        var setvendoremail_notification = new Orderto_pna_admin_emailreqNew()
                                                        {
                                                            item = iteminfo.Item,
                                                            member_fnmae = memberinfo.FullName,
                                                            member_phone = memberinfo.PhoneNumbers,
                                                            orderdate = logdate.ToString(),
                                                            orderid = OrderID,
                                                            service_category = order.Service_category,
                                                            totalpay = Convert.ToDouble(getDiscounted_price),
                                                            vendoremail = vendorinfo.Email,
                                                            vendorname = vendorinfo.companyName,
                                                            sendEmailType = SendEmailType.Vendor
                                                        };
                                                        var setpna_adminemail_notification = new Orderto_pna_admin_emailreqNew()
                                                        {
                                                            item = iteminfo.Item,
                                                            member_fnmae = memberinfo.FullName,
                                                            member_phone = memberinfo.PhoneNumbers,
                                                            orderdate = logdate.ToString(),
                                                            orderid = OrderID,
                                                            service_category = order.Service_category,
                                                            totalpay = Convert.ToDouble(getDiscounted_price),
                                                            vendoremail = vendorinfo.Email,
                                                            vendorname = vendorinfo.companyName,
                                                            sendEmailType = SendEmailType.Admin
                                                        };
                                                        //send mail to vendor

                                                        var vendEmail = await SendEmailmailnotification(setvendoremail_notification);
                                                        if (vendEmail.Code == "00")
                                                        {
                                                            //send mail to Pna Admin
                                                            var respo = await SendEmailmailnotification(setpna_adminemail_notification);
                                                            if (respo.Code == "00")
                                                            {
                                                                emailSent = true;
                                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "SUCCESS", setDTO);
                                                            }
                                                            else
                                                            {
                                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Order recorded but sending of email not successful", setDTO);
                                                            }

                                                            var syslog = new Sys_logReq()
                                                            {
                                                                channel = _appChannel.Channel1,
                                                                description = $"Member {memberinfo.FullName} made booking order_id:{OrderID} from vendor: {vendorinfo.companyName}",
                                                                UserID = memberinfo.Email,
                                                                Usertype = _appSystemType.Type7
                                                            };
                                                            var sys_log = await _systemUtility.sys_Log(syslog);
                                                        }


                                                    }
                                                    else
                                                    {

                                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"could not process Order, try again");
                                                    }

                                                }
                                            }
                                            else if (products.GetType() == typeof(EventDTO) == true)
                                            {
                                                foreach (var iteminfo in productDTOs)
                                                {
                                                    //get discounted amount
                                                    var getDiscounted_price = iteminfo.Item_price - (iteminfo.Item_price * iteminfo.Item_Discount / 100);

                                                    var setproduct_itemto_order = new Order()
                                                    {
                                                        Demand_Type = order.Demand_Type,
                                                        Item_price = iteminfo.Item_price,
                                                        Discounted_amount = getDiscounted_price,
                                                        Item = iteminfo.Item,
                                                        OrderId = OrderID,
                                                        MembershipID = order.MembershipID,
                                                        // number_of_persons = order.Number_of_persons,
                                                        IsItem_paid = false,
                                                        IsVendorCredited = false,
                                                        PaymentMethod = "Wallet",
                                                        //reservation_date = order.Reservation_date,
                                                        Service_category = order.Service_category,
                                                        Service_type = order.Service_type,
                                                        Table_type = order.Table_type,
                                                        Total_payable_amount = getDiscounted_price,
                                                        Order_date = logdate,
                                                        Vendor_code = order.Vendor_code,
                                                    };
                                                    var setDTO = new Order_View()
                                                    {
                                                        Item = iteminfo.Item,
                                                        Item_price = iteminfo.Item_price,
                                                        Item_Discount = iteminfo.Item_Discount.ToString() + "%",

                                                        Item_DiscountedPrice = getDiscounted_price,
                                                        OrderReference = OrderID,
                                                        Total_payable = getDiscounted_price
                                                    };
                                                    OrderInsert = connlogon.Insert(setproduct_itemto_order);
                                                    srrvcate = order.Service_category;
                                                    //check if record inserted well
                                                    if (OrderInsert >= 1)
                                                    {
                                                        // send Order reciept to email member
                                                        var subject = $"Your {vendorinfo.companyName} Order {OrderID} - has been confirmed.";
                                                        string path1 = @"Resources\EmailTemplate\Vending_email.html";
                                                        string fullPath;
                                                        fullPath = Path.GetFullPath(path1);
                                                        StreamReader str = new StreamReader(fullPath);
                                                        string MailText = str.ReadToEnd();
                                                        str.Close();
                                                        MailText = MailText.Replace("[fname]", memberinfo.FullName.ToUpper());
                                                        MailText = MailText.Replace("[vendorname]", vendorinfo.companyName);
                                                        MailText = MailText.Replace("[orderid]", OrderID);
                                                        MailText = MailText.Replace("[item]", iteminfo.Item);
                                                        MailText = MailText.Replace("[item_price]", iteminfo.Item_price.ToString("N", CultureInfo.CurrentCulture));
                                                        MailText = MailText.Replace("[discount]", iteminfo.Item_Discount.ToString() + "%");
                                                        MailText = MailText.Replace("[orderdate]", logdate.ToString());
                                                        MailText = MailText.Replace("[service_category]", order.Service_category);
                                                        MailText = MailText.Replace("[totalpay]", getDiscounted_price.ToString("N", CultureInfo.CurrentCulture));
                                                        var SendEmailApi = new SendEmailApiReq()
                                                        {
                                                            htmlBody = MailText,
                                                            MailSubject = subject,
                                                            MailTo = memberinfo.Email
                                                        };

                                                        await _notificationServices.sendmail(SendEmailApi);
                                                        var setvendoremail_notification = new Orderto_pna_admin_emailreqNew()
                                                        {
                                                            item = iteminfo.Item,
                                                            member_fnmae = memberinfo.FullName,
                                                            member_phone = memberinfo.PhoneNumbers,
                                                            orderdate = logdate.ToString(),
                                                            orderid = OrderID,
                                                            service_category = order.Service_category,
                                                            totalpay = Convert.ToDouble(getDiscounted_price),
                                                            vendoremail = vendorinfo.Email,
                                                            vendorname = vendorinfo.companyName,
                                                            sendEmailType = SendEmailType.Vendor
                                                        };
                                                        var setpna_adminemail_notification = new Orderto_pna_admin_emailreqNew()
                                                        {
                                                            item = iteminfo.Item,
                                                            member_fnmae = memberinfo.FullName,
                                                            member_phone = memberinfo.PhoneNumbers,
                                                            orderdate = logdate.ToString(),
                                                            orderid = OrderID,
                                                            service_category = order.Service_category,
                                                            totalpay = Convert.ToDouble(getDiscounted_price),
                                                            vendoremail = vendorinfo.Email,
                                                            vendorname = vendorinfo.companyName,
                                                            sendEmailType = SendEmailType.Admin
                                                        };
                                                        //send mail to vendor

                                                        var vendEmail = await SendEmailmailnotification(setvendoremail_notification);
                                                        if (vendEmail.Code == "00")
                                                        {
                                                            //send mail to Pna Admin
                                                            var respo = await SendEmailmailnotification(setpna_adminemail_notification);
                                                            if (respo.Code == "00")
                                                            {
                                                                emailSent = true;
                                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "SUCCESS", setDTO);
                                                            }
                                                            else
                                                            {
                                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Order recorded but sending of email not successful", setDTO);
                                                            }

                                                            var syslog = new Sys_logReq()
                                                            {
                                                                channel = _appChannel.Channel1,
                                                                description = $"Member {memberinfo.FullName} made booking order_id:{OrderID} from vendor: {vendorinfo.companyName}",
                                                                UserID = memberinfo.Email,
                                                                Usertype = _appSystemType.Type7
                                                            };
                                                            var sys_log = await _systemUtility.sys_Log(syslog);
                                                        }


                                                    }
                                                    else
                                                    {

                                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"could not process Order, try again");
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "No such Item");
                                }

                            }
                            else
                            {
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "No such Vendor");
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "No such member");
                        }



                    }

                }

            }
            catch (Exception ex)
            {

                await _systemUtility.JsonErrorLog($"[PNAAPI][member new Order][Response] => {ex.Message} | [request]=> " +
                                               JsonConvert.SerializeObject(order) + $"| [requestId]=> {requestId}");
            }
            return new ServerResponse
            {
                Code = response.Code,
                Data = response.Data,
                IsSuccessful = emailSent,
                Message = response.Message,
                RequestId = response.RequestId,
                Status = emailSent == true ?
                StatusCodesExtended.Status201Created.ToString() : StatusCodesExtended.Status411NotCreated.ToString()
            };

        }


        /*  var sql = "Select p.*, b.* from Products p, Billers b where p.biller_Id = b.id and p.productcode = @productcode";
            return (await _connection.QueryAsync<Product, Biller, Product>(sql,
                (p, b) =>
                {
                    p.Biller = b;
                    return p;
                }, new { productCode},
                splitOn: "ID"
                )).FirstOrDefault();*/
        #region Old
        //public async Task<object> CreateOrder_(OrderDTO order, object products)
        //{
        //    try
        //    {
        //        List<Order> orders = new List<Order>();

        //        if ((products.GetType() == typeof(List<OrderDetail>)) == true)
        //        {
        //            var prod = (List<OrderDetail>)products;

        //            foreach (var pro in prod)
        //            {
        //                double discount = pro.Discounted_amount;
        //                orders.Add(new Order
        //                {
        //                    Demand_Type = order.Demand_Type,
        //                    Discount = discount,




        //                });
        //            }
        //        }
        //        else if (products.GetType() == typeof(ProductDTO) == true)
        //        {

        //        }
        //        else
        //        {
        //            return new ServerResponse
        //            {

        //                Data = null,
        //                Code = ResponseCodes.UNSUCCESSFUL,
        //                IsSuccessful = true,
        //                RequestId = CustomFunctions.RequestID(),
        //                Status = StatusCodesExtended.Status413Unsessful.ToString()


        //            };
        //        }

        //        string sql = "SELECT * FROM DB_A57DC4_pnaDb_GymSpaProductCategory WHERE Name=@Name;";
        //        using (IDbConnection con = new SqlConnection(_connectionstr))
        //        {
        //            if (con.State == ConnectionState.Closed) { con.Open(); }
        //            //var entity = (OrderDTO)orderDto;

        //            var setproduct_itemto_order = new Order()
        //            {
        //                Demand_Type = order.Demand_Type,

        //                It = iteminfo.item_price,
        //                discounted_amount = getDiscounted_price,
        //                item = iteminfo.item,
        //                OrderId = OrderID,
        //                MembershipID = makeOrderReq.MembershipID,
        //                number_of_persons = makeOrderReq.number_of_persons,
        //                IsItem_paid = false,
        //                IsVendorCredited = false,
        //                paymentMethod = "Wallet",
        //                reservation_date = makeOrderReq.reservation_date,
        //                service_category = makeOrderReq.service_category,
        //                service_type = makeOrderReq.service_type,
        //                table_type = makeOrderReq.table_type,
        //                total_payable_amount = getDiscounted_price,
        //                order_date = logdate,
        //                Vendor_code = makeOrderReq.Vendor_code
        //            };
        //            var setDTO = new NightLife_OrdersDTO()
        //            {
        //                ItemName = iteminfo.item,
        //                ItemPrice = iteminfo.item_price,
        //                ItemDiscount = iteminfo.item_discount.ToString() + "%",
        //                Item_DiscountedPrice = getDiscounted_price,
        //                OrderReference = OrderID,
        //                Total_payable = getDiscounted_price
        //            };
        //            var OrderInsert = connlogon.Insert(setproduct_itemto_order);

        //            return new ServerResponse
        //            {

        //                Data = data,
        //                Code = ResponseCodes.SUCCESS,
        //                IsSuccessful = true,
        //                RequestId = CustomFunctions.RequestID(),
        //                Status = StatusCodesExtended.Status200OK.ToString(),
        //                Message = "Name already exists"


        //            };

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion
        public async Task<ServerResponse> DeleteOrder(long orderId, object type)
        {
            ServerResponse sr = new ServerResponse();
            try
            {



                string sql = string.Empty;
                object obj = new object();
                if (type.GetType() == typeof(OrderDTO))
                {

                    using (IDbConnection con = new SqlConnection(_connectionstr))
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        //if (type == typeof(OrderDTO))
                        //{   }
                        sql = "Update Order  Deleted=1 where Id=@Id;";
                        var data = await con.QuerySingleOrDefaultAsync<Order>(sql, new { Id = orderId });

                        var trans = con.BeginTransaction();
                        if (data != null)
                        {
                            var del = await con.UpdateAsync<Order>(data, trans);
                            if (del == true)
                            {
                                var dataReturnd = _mapper.Map<OrderDTO>(data);
                                trans.Commit();
                                sr = new ServerResponse
                                {
                                    Code = ResponseCodes.SUCCESS,
                                    Data = dataReturnd,
                                    IsSuccessful = true,
                                    Message = $"Order with code {data.OrderCode} was successfully removed",
                                    RequestId = CustomFunctions.RequestID(),
                                    Status = StatusCodesExtended.Status212Deleted.ToString()

                                };
                            }
                            else
                            {
                                trans.Rollback();
                                sr = new ServerResponse
                                {
                                    Code = ResponseCodes.UNSUCCESSFUL,
                                    Data = null,
                                    IsSuccessful = false,
                                    Message = $"Order with code {data.OrderCode} was not successfully and transacion rolledback",
                                    RequestId = CustomFunctions.RequestID(),
                                    Status = StatusCodesExtended.Status500InternalServerError.ToString()

                                };
                            }
                        }



                    }
                }
                else if (type.GetType() == typeof(NightLife_OrdersDTO))
                {
                    sr = new ServerResponse
                    {
                        Code = ResponseCodes.ERROR,
                        Data = null,
                        IsSuccessful = false,
                        Message = $"Not implemeted for NightLife_Orders",
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status304NotModified.ToString()

                    };
                }
            }
            catch (Exception ex)
            {
                sr = new ServerResponse
                {
                    Code = ResponseCodes.ERROR,
                    Data = null,
                    IsSuccessful = false,
                    Message = $"Order  was not successfully ; an error occured ",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status304NotModified.ToString()

                };


            }
            return sr;

        }

        public async Task<ServerResponse> EditOrder(object order, long orderId)
        {
            ServerResponse sr = new ServerResponse();
            try
            {



                string sql = string.Empty;
                object obj = new object();
                if (order.GetType() == typeof(Order))
                {

                    using (IDbConnection con = new SqlConnection(_connectionstr))
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        var orderData = (OrderDTO)order;



                        sql = "Update Order  Deleted=1 where Id=@Id;";
                        var data = await con.QuerySingleOrDefaultAsync<Order>(sql, new { Id = orderId });
                        data.IsItem_paid = orderData.IsItem_paid;
                        data.IsVendorCredited = orderData.IsVendorCredited;
                        data.Service_category = orderData.Service_category;
                        data.Service_type = orderData.Service_type;
                        data.TotalCost = orderData.TotalCost;

                        var trans = con.BeginTransaction();
                        if (data != null)
                        {
                            var del = await con.UpdateAsync<Order>(data, trans);
                            if (del == true)
                            {
                                var dataReturnd = _mapper.Map<OrderDTO>(data);
                                trans.Commit();
                                sr = new ServerResponse
                                {
                                    Code = ResponseCodes.SUCCESS,
                                    Data = dataReturnd,
                                    IsSuccessful = true,
                                    Message = $"Order with code {data.OrderCode} was successfully removed",
                                    RequestId = CustomFunctions.RequestID(),
                                    Status = StatusCodesExtended.Status212Deleted.ToString()

                                };
                            }
                            else
                            {
                                trans.Rollback();
                                sr = new ServerResponse
                                {
                                    Code = ResponseCodes.UNSUCCESSFUL,
                                    Data = null,
                                    IsSuccessful = false,
                                    Message = $"Order with code {data.OrderCode} was not successfully and transacion rolledback",
                                    RequestId = CustomFunctions.RequestID(),
                                    Status = StatusCodesExtended.Status500InternalServerError.ToString()

                                };
                            }
                        }


                    }
                }
                else if (order.GetType() == typeof(NightLife_OrdersDTO))
                {
                    sr = new ServerResponse
                    {
                        Code = ResponseCodes.ERROR,
                        Data = null,
                        IsSuccessful = false,
                        Message = $"Not yet implemeted for NightLife_Orders",
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status304NotModified.ToString()

                    };
                }
            }
            catch (Exception ex)
            {
                sr = new ServerResponse
                {
                    Code = ResponseCodes.ERROR,
                    Data = null,
                    IsSuccessful = true,
                    Message = $"Order  was not successfully ; an error occured ",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status304NotModified.ToString()

                };


            }
            return sr;
        }

        public Task<ServerResponse> OrderNotification(object items)
        {
            throw new NotImplementedException();
        }

        public async Task<ServerResponse> OrderPaymentReceipt(ReceiptDTO receipt)
        {
            ServerResponse sr = new ServerResponse();
            if (receipt != null)
            {

                try
                {



                    string sql = string.Empty;



                    using (IDbConnection con = new SqlConnection(_connectionstr))
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        //if (type == typeof(OrderDTO))
                        //{   }
                        sql = "INSERT INTO Receipt (Total,TransactionDate,TransactionRference,ServiceCayegory,OrderId,PnaMemberId,VendorId) Values " +
                        "(@Total,@TransactionDate,@TransactionRference,@ServiceCayegory,@OrderId,@PnaMemberId,@VendorId);";
                        var data = await con.QuerySingleOrDefaultAsync<OrderDTO>(sql, new { receipt });

                        var trans = con.BeginTransaction();
                        if (data != null && data.Id > 0)
                        {



                            trans.Commit();
                            sr = new ServerResponse
                            {
                                Code = ResponseCodes.SUCCESS,
                                Data = data,
                                IsSuccessful = true,
                                Message = $"Receipt was successfully created",
                                RequestId = CustomFunctions.RequestID(),
                                Status = StatusCodesExtended.Status212Deleted.ToString()

                            };

                        }
                        else
                        {

                            trans.Rollback();
                            sr = new ServerResponse
                            {
                                Code = ResponseCodes.SUCCESS,
                                Data = data,
                                IsSuccessful = false,
                                Message = $"Receipt was not successfully created",
                                RequestId = CustomFunctions.RequestID(),
                                Status = StatusCodesExtended.Status212Deleted.ToString()

                            };

                        }



                    }


                }
                catch (Exception ex)
                {
                    sr = new ServerResponse
                    {
                        Code = ResponseCodes.ERROR,
                        Data = null,
                        IsSuccessful = false,
                        Message = $"Recipt  was not successfully created ; an error occured ",
                        RequestId = CustomFunctions.RequestID(),
                        Status = StatusCodesExtended.Status304NotModified.ToString()

                    };


                }

            }
            else
            {
                sr = new ServerResponse
                {
                    Code = ResponseCodes.ERROR,
                    Data = CustomFunctions.RequestID(),
                    IsSuccessful = false,
                    Message = $"Please provide all the required fields ",
                    RequestId = CustomFunctions.RequestID(),
                    Status = StatusCodesExtended.Status304NotModified.ToString()

                };

            }
            return sr;
        }

        //Task<T2> IGymSpaService<ServerResponse>.AutoMappingObjs<T2, T3>(T2 source, T3 destination)
        //{
        //    var data = _mapper.Map<T2>(typeof(T3),typeof(T2));
        //}
    }
}
