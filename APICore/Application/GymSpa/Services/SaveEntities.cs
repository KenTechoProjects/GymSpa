using APICore.Application.GymSpa.Connectors;
using Dapper;
using Domain.Application.GymSpa.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Utilities.Service;
using Dapper;
using Domain.Application.EventTickets.DTO;

namespace APICore.Application.GymSpa.Services
{
    public class SaveEntities : ISaveEntities
    {
        private readonly IConnectionStrings _config;
        private readonly IDapperr _dapper;
        public SaveEntities(IDapperr dapper, IConnectionStrings config)
        {
            _config = config;
            _dapper = dapper;
        }
        public async Task<AppointmentDTO> CreateAppointment(AppointmentDTO model)
        {

            AppointmentDTO data = new AppointmentDTO();


            string sql = "INSERT INTO GymSpaAppointment (AppointmentDate,Time,Duration,PnaVendorId,MemberId,Cost,Discount) OUTPUT INSERTED.* " +
                " VALUES (@AppointmentDate,@Time,@Duration,@PnaVendorId,@MemberId,@Cost,@Discount)";

            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();
                data = await con.QueryFirstOrDefaultAsync<AppointmentDTO>(sql, model, trans);
                return data;
            }


        }

        public async Task<DiscountLevelDTO> CreateDiscountLevel(DiscountLevelDTO entity)
        {



            String sql = "INSERT INTO DiscountLevel (Discount,Discount_Level,ProductID,PnaVendorID) OUTPUT INSERTED.* VALUES "+
                "(@Discount,@Discount_Level,@ProductID,@PnaVendorID);";
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();
                var data = await con.QueryFirstOrDefaultAsync<DiscountLevelDTO>(sql, entity, trans);
                return data;
            }


        }

        public async Task<OrderDTO> CreateOrder(OrderDTO entity) 
        {



            string sql = "INSERT INTO DB_A57DC4_pnaDb_Order (Vendor_code,OrderId,Order_date,VendorID,TotalCost,ProductCount,Discount,NetCost,MembershipID," +
                "Service_type,Demand_type,Service_category,Totalpayable_amount,IsVendorCredited,IsItem_paid,Payment_status,PaymentMethod,PaymentReference,Payment_date) OUTPUT INSERTED.* VALUES " +
                "(@Vendor_code,@OrderId,@Order_date,@VendorID,@TotalCost,@ProductCount,@Discount,@NetCost,@MembershipID,"+
                "@Service_type,@Demand_type,@Service_category,@Totalpayable_amount,@IsVendorCredited,@IsItem_paid,@Payment_status,@PaymentMethod,@PaymentReference,@Payment_date)";
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();
                var data = await con.QueryFirstOrDefaultAsync<OrderDTO>(sql, entity, trans);
                return data;
            }


        }

        public async Task<OrderDetailDTO> CreateOrderDetail(OrderDetailDTO entity)
        {
          
        string sql = "INSERT INTO OrderDetail (Item,Item_price ,Order_date ,Discounted_amount " +
                ",OrderId,DiscountedInPercent,ProductId,MembershipId,Qunatity) OUTPUT INSERTED. VALUES " +
                "(@Item,@Item_price ,@Order_date,@Discounted_amount" +
                ",@OrderId,@DiscountedInPercent,@ProductId,@MembershipId,@Qunatity)";
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();
                var data = await con.QueryFirstOrDefaultAsync<OrderDetailDTO>(sql, entity, trans);
                return data;
            }


        }

        public async Task<ProductDTO> CreateProduct(ProductDTO entity)
        {
            string sql = "INSERT INTO  Product (Name,Maker,BarCordePath,ProductionYear,Vendor_code,Item_price,Item_image,Date_Created,Item_Code" +
                ",Item_Discount,ProductCategoryId,DiscountLevelId) OUTPUT INSERTED.* VALUES " +
                " (@Name,@Maker,@BarCordePath,@ProductionYear,@Vendor_code,@Item_price,@Item_image,@Date_Created,@Item_Code" +
                ",@Item_Discount,@ProductCategoryId,@DiscountLevelId)";
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();


                var data = await con.QueryFirstOrDefaultAsync<ProductDTO>(sql, entity, trans);
                return data;

            }

        }





        public async Task<object> CreateProductCategory(ProductCategoryDTO entity)
        {
            string sql = "  INSERT INTO ProductCategory (Name,VendorId)  OUTPUT INSERTED.* VALUES " +
                   " (@Name,@VendorId) ;";

            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();


                var data = await con.QueryFirstOrDefaultAsync<ProductCategoryDTO>(sql, new { Name = entity.Name, VendorId = entity.VendorId }, trans);
                return data;


            }
            //var dbparams = new DynamicParameters();
            //dbparams.Add("Name", entity.Name, DbType.String);
            //dbparams.Add("VendorId", entity.VendorId, DbType.Int32);

            //var   data = await _dapper.Insert<ProductCategoryDTO>(sql, dbparams);

        }

        public async Task<SalesDTO> CreateSales(SalesDTO entity)
        {
            var sql = "INSERT INTO GymSpaSales (OrderId,PnaVendorID,DateSoled) OUTPUT INSERTED.* VALUES (@OrderId,@PnaVendorID,@DateSoled);";
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();
      var data = await  con.QueryFirstOrDefaultAsync<SalesDTO>(sql, entity,trans);
            return data;

            }


          
        }

        public async Task<WorkingDateDTO> CreateWorkingDate(WorkingDateDTO entity)
        {
            string sql = "INSERT INTO GymSpaDates (Availabledate,IsAvailable,IsClosed,PnaVendorID,IsActive) OUTPUT INSERTED.* "+
                "(@Availabledate,@IsAvailable,@IsClosed,@PnaVendorID,@IsActive)";
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();
                var data = await con.QueryFirstOrDefaultAsync<WorkingDateDTO>(sql, entity, trans);
                return data;

            }

       
        }

        public async Task<StockDTO> CreateStock(StockDTO entity)
        {
            string sql = "INSERT INTO DB_A57DC4_pnaDb_Stock (TotalStock,TotalSold,TotalReTurned,TotalSpoilt,PnaVendorID) OUTPUT INSERTED.* VALUES " +
                "(@TotalStock,@TotalSold,@TotalReTurned,@TotalSpoilt,@PnaVendorID)";
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();
                var data = await con.QueryFirstOrDefaultAsync<StockDTO>(sql, entity, trans);
                return data;

            }

           
        }

        public async Task<ErrorLogDbDTO> CreateErrorLogDb(ErrorLogDbDTO entity)
        {
            string sql = "INSET INTO  ErrorLogDb (Exception,DateCreated,DateResolved,PnaVendorID) OUTPUT INSERTED.* (@Exception,@DateCreated,@DateResolved,@PnaVendorID)";
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();
                var data = await con.QueryFirstOrDefaultAsync<ErrorLogDbDTO>(sql, entity, trans);
                return data;

            }
 
        }
        public async Task<StaffDTO> CreateStaff(StaffDTO entity)
        {
            string sql = "INSERT INTO  Staff (StaffID,IncreamentedId,VendorId,Title,FullName,PhoneNumbers,DateofBirth,Gender,ProfilePicture,CountryofResidence"+
                ",Profession,State,City,Email,WalletId,QRcode,MaritalStatus,ActivationCode,IsActivated) OUTPUT INSERTED.* VALUES "+
                "(@StaffID,@IncreamentedId,@VendorId,@Title,@FullName,@PhoneNumbers,@DateofBirth,@Gender,@ProfilePicture,@CountryofResidence"+
                ",@Profession,@State,@City,@Email,@WalletId,@QRcode,@MaritalStatus,@ActivationCode,@IsActivated)";
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();
                var data = await con.QueryFirstOrDefaultAsync<StaffDTO>(sql, entity, trans);
                return data;

            }

         
        }
        public async Task<ErrorLogSolutionDTO> CreateErrorLogSolution(ErrorLogSolutionDTO entity)
        {
            string sql = "INERT INTO ErrorLogSolution (ErrorLogDbId,DateCreated,Solution) OUTPUT INSERTED.* VALUES (@ErrorLogDbId,@DateCreated,@Solution);";
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var trans = con.BeginTransaction();
                var data = await con.QueryFirstOrDefaultAsync<ErrorLogSolutionDTO>(sql, entity, trans);
                return data;

            }

            
        }

        public async Task<DutyRoasterDTO> CreateDutyRoaster(DutyRoasterDTO model)
        {
            string sql = "Insert into DutyRoaster (StaffId,DateCreated,StartDate,EndDate,VendorId,RequestorId) OUTPUT INSERTED.* VALUES " +
                " (@StaffId,@DateCreated,@StartDate,@EndDate,@VendorId,@RequestorId)";


            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var data = await con.QueryFirstOrDefaultAsync<DutyRoasterDTO>(sql, model);
                return data;
            }

        }


        public async Task<DurationDTO> CreateDuration(DurationDTO model)
        {
            string sql = "Insert into Duration (Time,IsAvailable,IsClosed,IsActive,VendorCode,PnaVendorId,AlotedDate) OUTPUT INSERTED.* VALUES " +
                " (@Time,@IsAvailable,@IsClosed,@IsActive,@VendorCode,@PnaVendorId,@AlotedDate)";


            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var data = await con.QueryFirstOrDefaultAsync<DurationDTO>(sql, model);
                return data;
            }

        }

        public async Task<BaseServiceDTO> CreateServices(BaseServiceDTO model)
        {
            string sql = "Insert into BaseService (ServiceName) OUTPUT INSERTED.* VALUES " +
               " (@ServiceName)";


            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var data = await con.QueryFirstOrDefaultAsync<BaseServiceDTO>(sql, model);
                return data;
            }

        }

        public async Task<PnaVendorServiceDTO> JoinVendorToService(PnaVendorServiceDTO model)
        {
            string sql = "Insert into PnaVendorService (SerciveId,VendorId) OUTPUT INSERTED.* VALUES " +
               " (@SerciveId,@VendorId)";


            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var data = await con.QueryFirstOrDefaultAsync<PnaVendorServiceDTO>(sql, model);
                return data;
            }
        }


        public async Task<EventDTO> CreateEvent(EventDTO model)
        {
            string sql = "Insert into Event (Name,ItemCode,TimeOfEvent,DateOfEvent,VendorId,NumberOfPerson,Amount,Discount,EventTicketManagerId) OUTPUT INSERTED.* VALUES " +
               " (@Name,@ItemCode,@TimeOfEvent,@DateOfEvent,@VendorId,@NumberOfPerson,@Amount,@Discount,@EventTicketManagerId;";


            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var data = await con.QueryFirstOrDefaultAsync<EventDTO>(sql, model);
                return data;
            }
        }
        public async Task<EventTicketDTO> CreateEventTicket(EventTicketDTO model)
        {

            string sql = "Insert into EventTicket (Ticket,TicketDate,PnaMemberId,VendorId,EventTicketManagerId) OUTPUT INSERTED.* VALUES " +
               " (@Ticket,@TicketDate,@PnaMemberId,@VendorId,@EventTicketManagerId)";


            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var data = await con.QueryFirstOrDefaultAsync<EventTicketDTO>(sql, model);
                return data;
            }
        }
        public async Task<EventTicketManagerDTO> CreateEventTicketManager(EventTicketManagerDTO model)
        {
            string sql = "Insert into EventTicketManager (StaffId,VendorId,ManagerCode) OUTPUT INSERTED.* VALUES " +
            " (@StaffId,@VendorId,@ManagerCode)";


            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var data = await con.QueryFirstOrDefaultAsync<EventTicketManagerDTO>(sql, model);
                return data;
            }
        }

       public async Task<EmergencyTicketManagerDTO> CreateEmergencyEventTicketManager(EmergencyTicketManagerDTO model)
        {
            string sql = "Insert into EmergencyTicketManager (IsEmergency,ManagerCode,Fullname,Address,Email,Phone,VendorId,EventTicketManagerId) OUTPUT INSERTED.* VALUES " +
            " (@IsEmergency,@ManagerCode,@Fullname,@Address,@Email,@Phone,@VendorId,@EventTicketManagerId)";


            using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var data = await con.QueryFirstOrDefaultAsync<EmergencyTicketManagerDTO>(sql, model);
                return data;
            }
        }

    }



}
