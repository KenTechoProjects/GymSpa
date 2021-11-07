using APICore.Application.GymSpa.Connectors;
using Dapper;
using Domain.Application.EventTickets.DTO;
using Domain.Application.GymSpa.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace APICore.Application.GymSpa.Services
{
    public class GetEntities : IGetEntities
    {
        private readonly IConnectionStrings _config;
        private readonly IDapperr _dapper;
        private readonly Logger _logger;
        public GetEntities(IDapperr dapper, IConnectionStrings config, Logger logger)
        {
            _config = config;
            _dapper = dapper;
            _logger = logger;
        }





        public async Task<IEnumerable<ErrorLogDbDTO>> GetErrorLogDbd(long Id, int vendorId)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from ErrorLogDb where Id=@Id and PnaVendorID=@vendorId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<ErrorLogDbDTO>(sql, new { Id, vendorId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentd(long Id, int vendorId)
        {
            try
            {
                string sql = "Select * from DB_A57DC4_pnaDb_Appointment where Id=@Id and PnaVendorID=@vendorId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<AppointmentDTO>(sql, new { Id, vendorId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }

        }

        public async Task<IEnumerable<DiscountLevelDTO>> GetDiscountLeveld(long Id, int vendorId)
        {

            try
            {
                string sql = "Select * from DiscountLevel where Id=@Id and PnaVendorID=@vendorId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<DiscountLevelDTO>(sql, new { Id, vendorId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<ErrorLogSolutionDTO>> GetErrorLogSolutiond(long Id, long errorLogDbId)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from ErrorLogSolution where Id=@Id and ErrorLogDbId=@errorLogDbId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<ErrorLogSolutionDTO>(sql, new { Id, errorLogDbId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrderd(long Id, int vendorId)
        {
            try
            {
                string sql = "Select o.*, d.* from DB_A57DC4_pnaDb_Order o, OrderDetail d where Id=@Id and d.OrderId=o.Id and o.VendorID=@vendorId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<OrderDTO, List<OrderDetailDTO>, OrderDTO>(sql,
                        (o, d) =>
                        {
                            o.OrderDetails = d;
                            return o;
                        },

                        new { Id, vendorId }, splitOn: "Id");
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }

        }


        public async Task<IEnumerable<OrderDTO>> GetOrderd(long Id, int vendorId, string membershipId)
        {
            try
            {
                string sql = "Select o.*, d.* from DB_A57DC4_pnaDb_Order o, OrderDetail d where Id=@Id and d.OrderId=o.Id and o.VendorID=@vendorId and MembershipID=@membershipId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<OrderDTO, List<OrderDetailDTO>, OrderDTO>(sql,
                        (o, d) =>
                        {
                            o.OrderDetails = d;
                            return o;
                        },

                        new { Id, vendorId, membershipId }, splitOn: "Id");
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }

        }




        public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetaild(long Id, long orderId, int vendorId)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select r.*,s.* from OrderDetail r, DB_A57DC4_pnaDb_Order s where r.Id=@Id and r.OrderId=@orderId and s.VendorID=@vendorId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<OrderDetailDTO, OrderDTO, OrderDetailDTO>(sql,
                        (r, s) =>
                        {
                            r.Order = s;
                            return r;

                        },

                        new { Id, orderId, vendorId }, splitOn: "Id");
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductd(long Id, long categoryId, string vendorCode)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from Product where Id=@Id and ProductCategoryId=@categoryId and Vendor_code=@vendorCode";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<ProductDTO>(sql, new { Id, categoryId, vendorCode });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<ProductCategoryDTO>> GetProductCategoryd(long Id, int vendorId)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select c.*,p.* from ProductCategory c, Product p where c.Id=@Id and c.VendorId=@vendorId and p.ProductcategoryId=c.Id";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<ProductCategoryDTO, List<ProductDTO>, ProductCategoryDTO>(sql,
                        (c, p) =>
                        {
                            c.Products = p;
                            return c;
                        },

                        new { Id, vendorId }, splitOn: "Id");
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<StaffDTO>> GetStaffd(string staffId, string vendorCode)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from Staff where StaffID=@staffId and VendorCode=@vendorCode";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<StaffDTO>(sql, new { staffId, vendorCode });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<StockDTO>> GetStockd(long Id, int vendorId)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from DB_A57DC4_pnaDb_Stock where Id=@Id and PnaVendorID=@vendorId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<StockDTO>(sql, new { Id, vendorId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<WorkingDateDTO>> GetWorkingDated(long Id, string vendorCode)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from DB_A57DC4_pnaDb_GStock where Id=@Id and VendorCode=@vendorCode";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<WorkingDateDTO>(sql, new { Id, vendorCode });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<SalesDTO>> GetSale(long Id, long orderId, int vendorId)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from GymSpaSales where Id=@Id and PnaVendorID=@vendorId and OrderId=@orderId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<SalesDTO>(sql, new { Id, vendorId, orderId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }


















        public async Task<IEnumerable<ErrorLogDbDTO>> GetErrorLogDbds()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from ErrorLogDb where;";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<ErrorLogDbDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentds()
        {
            try
            {
                string sql = "Select * from DB_A57DC4_pnaDb_Appointment ";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<AppointmentDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<DiscountLevelDTO>> GetDiscountLevelds()
        {
            try
            {
                string sql = "Select * from DiscountLevel; ";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<DiscountLevelDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<ErrorLogSolutionDTO>> GetErrorLogSolutionds()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from ErrorLogSolution ;";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<ErrorLogSolutionDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrderds()
        {
            try
            {
                string sql = "Select * from DB_A57DC4_pnaDb_Order  ";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<OrderDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }

        }

        public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetailds()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from OrderDetail  ;";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<OrderDetailDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductds()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from DB_A57DC4_pnaDb_GymSpaProduct ";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<ProductDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<ProductCategoryDTO>> GetProductCategoryds()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from DB_A57DC4_pnaDb_GymSpaProductCategory ;";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<ProductCategoryDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<StaffDTO>> GetStaffds()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from Staff;";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<StaffDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<StockDTO>> GetStockds()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from DB_A57DC4_pnaDb_Stock ;";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<StockDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<WorkingDateDTO>> GetWorkingDateds()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from GymSpaDates ;";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<WorkingDateDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }
        public async Task<IEnumerable<SalesDTO>> GetSales()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from GymSpaSales;";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<SalesDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        //public async Task<IEnumerable<DutyRoasterDTO>> GetDutyRoaster(long Id, int vendorId)
        //{
        //    try
        //    {
        //        // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
        //        string sql = "Select * from DutyREoaster  where VendorId=@vendorId and Id=@Id;";
        //        using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
        //        {
        //            if (con.State == ConnectionState.Closed) { con.Open(); }
        //            var data = await con.QueryAsync<DutyRoasterDTO>(sql,new { Id,vendorId});
        //            return data;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("An error occured please check the log file");
        //    }
        //}

        public async Task<IEnumerable<DutyRoasterDTO>> GeDutyRoasters()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from DutyREoaster  ;";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<DutyRoasterDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<DutyRoasterDTO>> GetDutyRoaster(long Id, int vendorId, string staffId = "")
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = string.Empty;
                if (!string.IsNullOrWhiteSpace(staffId))
                { sql = "Select * from DutyREoaster  where VendorId=@vendorId and Id=@Id and StaffId=@staffId;"; }
                else
                {
                    sql = "Select * from DutyREoaster  where VendorId=@vendorId and Id=@Id ;";
                }

                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<DutyRoasterDTO>(sql, new { Id, vendorId, staffId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<DurationDTO>> GetDuration(long Id, int vendorId, DateTime dateAloted)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorIddateAloted
                string sql = "Select * from Duration where Id=@Id and PnaVendorID=@vendorId and AlotedDate=@orderId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<DurationDTO>(sql, new { Id, vendorId, dateAloted });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<IEnumerable<DurationDTO>> GetDurations()
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorIddateAloted
                string sql = "Select * from Duration;";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<DurationDTO>(sql);
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }



        public async Task<EventDTO> GetEvent(long Id)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from Event where Id=@Id";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryFirstOrDefaultAsync<EventDTO>(sql, new { Id });
                    return data;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("An error occured please check the log file");
            }
        }


        public async Task<EventDTO> GetEvent(long Id, int vendorId)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from Event where Id=@Id and VendorId=@vendorId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryFirstOrDefaultAsync<EventDTO>(sql, new { Id, vendorId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("An error occured please check the log file");
            }
        }
        public async Task<IEnumerable<EventDTO>> GetEvent(string Itemcode, int vendorId)
        {
            try
            {

                string sql = "Select * from Event where ItemCode=@Itemcode and VendorId=@vendorId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<EventDTO>(sql, new { Itemcode, vendorId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("An error occured please check the log file");
            }
        }
        public async Task<IEnumerable<EventDTO>> GetEvent(int vendorId, int EventTicketManagerId = 0, string itemCode = null)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = string.Empty;
                if (EventTicketManagerId > 0)
                {
                    sql = "Select * from Event where VendorId=@vendorId and EventTicketManagerId=@EventTicketManagerId";
                }
                else if (!string.IsNullOrWhiteSpace(itemCode))
                {
                    sql = "Select * from Event where VendorId=@vendorId and ItemCode=@itemCode";
                }
                else if (EventTicketManagerId >))
                {
                    sql = "Select * from Event where VendorId=@vendorId and ItemCode=@itemCode";
                }
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<EventDTO>(sql, new { EventTicketManagerId, vendorId, itemCode });
                    return data;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<EventTicketDTO> GetEventTicket(long Id, int vendorId)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from EventTicket where Id=@Id and VendorID=@vendorId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryFirstOrDefaultAsync<EventTicketDTO>(sql, new { Id, vendorId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("An error occured please check the log file");
            }
        }









        public async Task<IEnumerable<EventTicketDTO>> GetEventTickets(int vendorId)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from EventTicketManager where VendorId=@vendorId";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<EventTicketDTO>(sql, new { vendorId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("An error occured please check the log file");
            }
        }

        public Task<IEnumerable<EventTicketDTO>> GetEventTickets(int vendorId, int membershipId = 0, DateTime? dateofTicket = null, int EventTicketManagerId = 0)
        {
            try
            {
                // string sql = "Select * from GymSpaOrder where Id=@Id and VendorID=@vendorId";
                string sql = "Select * from EventTicketManager where VendorId=@vendorId;";
                if (membershipId > 0)
                {
                    sql = "Select * from EventTicketManager where VendorId=@vendorId and PnaMemberId=@membershipId;";
                }
                else if (membershipId > 0 && dateofTicket != null)
                {
                    sql = "Select * from EventTicketManager where VendorId=@vendorId and PnaMemberId=@membershipId and TicketDate=@dateofTicket;";
                }

                else if (dateofTicket != null)
                {
                    sql = "Select * from EventTicketManager where VendorId=@vendorId and TicketDate=@dateofTicket;";
                }
                else if (membershipId > 0 && dateofTicket != null && EventTicketManagerId > 0)
                {
                    sql = "Select * from EventTicketManager where VendorId=@vendorId and PnaMemberId=@membershipId " +
                        " and TicketDate=@dateofTicket and EventTicketManagerId=@EventTicketManagerId;";
                }

                else if (membershipId > 0 && EventTicketManagerId > 0)
                {
                    sql = "Select * from EventTicketManager where VendorId=@vendorId and PnaMemberId=@membershipId " +
                        "  and EventTicketManagerId=@EventTicketManagerId;";
                }

                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryAsync<EventTicketDTO>(sql, new { vendorId });
                    return data;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("An error occured please check the log file");
            }
        }

        public async Task<EventTicketManagerDTO> GetEventTicketManager(long id)
        {
            try
            {

                string sql = "Select * from EventTicketManager where Id=@Id";
                using (IDbConnection con = new SqlConnection(_config.GetConnectionString()))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    var data = await con.QueryFirstOrDefaultAsync<EventTicketManagerDTO>(sql, new { Id });
                    return data;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("An error occured please check the log file");
            }
        }

        public Task<IEnumerable<EventTicketManagerDTO>> GetEventTicketManagers(long Id, int vendorId, long staffId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventTicketManagerDTO>> GetEventTicketManagers(string managerCode, int vendorId, long staffId, long ticketManagerId)
        {
            throw new NotImplementedException();
        }

        public Task<EmergencyTicketManagerDTO> GetEmergencyTicketManager(long id)
        {
            throw new NotImplementedException();
        }

        public Task<EventTicketManagerDTO> GetEmergencyTicketManager(long Id, int vendorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventTicketManagerDTO>> GetEmergencyTicketManagers(string managerCode, int vendorId, long ticketManagerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventTicketManagerDTO>> GetEventTicketManager(string managerCode, int vendorId, long staffId, long ticketManagerId)
        {
            throw new NotImplementedException();
        }

        public Task<EventTicketManagerDTO> GetEmergencyTicketManagers(long Id, int vendorId)
        {
            throw new NotImplementedException();
        }
    }
}
