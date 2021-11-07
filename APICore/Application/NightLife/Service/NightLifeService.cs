using APICore.Application.Member.Interface;
using APICore.Application.NightLife.Interface;
using AppCore.Application.TransactionNotification;
using Dapper;
using Dapper.Contrib.Extensions;
using Domain.Application.Member.DTO;
using Domain.Application.SystemUtility.Sys_Logs;
using Domain.DTO.Notification.Email;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Notification.Interface;
using Persistence.Entities;
using SharedService.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;
using Utilities.Interface;
using Utilities.SystemActivity.Interface;

namespace APICore.Application.NightLife.Service
{
    public class NightLifeService : INightLifeService
    {
        private readonly Logger _logger;
        private readonly AppKeys _appKeys;
        private readonly IConfiguration _config;
        private readonly ISystemActivityService _systemUtility;
        private readonly AppSystemType _appSystemType;
        private readonly AppChannel _appChannel;
        private readonly ISharedService _sharedService;
        private readonly INotificationServices _notificationServices;
        private readonly BaseUrls _baseUrls;
        private readonly UIHttpClient _httpClient;
        private readonly ITransactionNotification _transactionNotification;
        private readonly IMemberService _memberService;

        public NightLifeService(Logger logger, IOptions<AppKeys> appKeys, IConfiguration configuration, ISystemActivityService systemUtility, IOptions<AppSystemType> appsystemtype, IOptions<AppChannel> channel, ISharedService sharedService, INotificationServices notificationServices, IOptions<BaseUrls> baseUrls, UIHttpClient httpClient, ITransactionNotification transactionNotification, IMemberService memberService)
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
            _memberService = memberService;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DB_A57DC4_PNADb"));
            }
        }

        public async Task<ResponseParam> Getallservice()
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                // Query the database for a requested

                using (IDbConnection conn = Connection)
                {
                    string selectQuery = @"SELECT * FROM [NightLifeProducts] ORDER BY Id DESC";

                    var Sys_Rolesresults = await conn.QueryAsync(selectQuery, new
                    {
                    });
                    int row = Sys_Rolesresults.Count();
                    if (row >= 1)
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Success", Sys_Rolesresults);
                    }
                    else if (row <= 1)
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "No Record found");
                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Oops. Something went wrong. Please try again later");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][Getallservice][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][Getallservice][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> Getallservice_byvendor(string vendorcode)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrWhiteSpace(vendorcode))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Vendorcode is Required");
                }
                else
                {
                    // Query the database for a requested

                    using (IDbConnection conn = Connection)
                    {
                        string selectQuery = $"SELECT * FROM [NightLifeProducts] where Vendor_code='{vendorcode}'ORDER BY Id DESC";

                        var Sys_Rolesresults = await conn.QueryAsync(selectQuery, new
                        {
                        });
                        int row = Sys_Rolesresults.Count();
                        if (row >= 1)
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Success", Sys_Rolesresults);
                        }
                        else if (row <= 1)
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "No Record found");
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Oops. Something went wrong. Please try again later");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][Getallservice][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][Getallservice_byvendor][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> GetNight_clubs()
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                // Query the database for a requested

                using (IDbConnection conn = Connection)
                {
                    string selectQuery = @"SELECT companyName,Companyaddress,Businesscategory,Vendor_code FROM [DB_A57DC4_pnaDb_Vendor] where Businesscategory='Night club' or Businesscategory='Club' ORDER BY Id DESC";

                    var Sys_Rolesresults = await conn.QueryAsync(selectQuery, new
                    {
                    });
                    int row = Sys_Rolesresults.Count();
                    if (row >= 1)
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Success", Sys_Rolesresults);
                    }
                    else if (row <= 1)
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "No Record found");
                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Oops. Something went wrong. Please try again later");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][Getallservice][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][GetClubs][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> Getserviceby_vendor(string vendor_code)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrWhiteSpace(vendor_code))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "vendor_code is required");
                }
                else
                {
                    // Query the database for a requested

                    using (IDbConnection conn = Connection)
                    {
                        string selectQuery = $"SELECT * FROM [NightLifeProducts] where Vendor_code={vendor_code} ORDER BY Id DESC";

                        var Sys_Rolesresults = await conn.QueryAsync(selectQuery, new
                        {
                        });
                        int row = Sys_Rolesresults.Count();
                        if (row >= 1)
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Success", Sys_Rolesresults);
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "No Record found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][Getallservice][Getallservice_By_vendorcode] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][Getallservice_By_vendorcode][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> Make_order(MakeNightLifeOrderReq makeOrderReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            var logmonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
            string Generatepassword = Guid.NewGuid().ToString("N").ToLower()
                        .Replace("1", "").Replace("o", "").Replace("0", "")
                        .Substring(0, 10);
            string ReferralCode = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 5);
            string OrderID = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 7);
            string WalletId = Helper.GenerateUniqueId(6);
            Random Rnd = new Random();
            string ActivationCode = (Rnd.Next(100000, 999999)).ToString();
            try
            {
                if (string.IsNullOrWhiteSpace(makeOrderReq.demand_type) || string.IsNullOrWhiteSpace(makeOrderReq.MembershipID) || string.IsNullOrWhiteSpace(makeOrderReq.service_category) || string.IsNullOrWhiteSpace(makeOrderReq.table_type) || string.IsNullOrWhiteSpace(makeOrderReq.Vendor_code) || string.IsNullOrWhiteSpace(makeOrderReq.item_code))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Most Fields are required");
                }
                else if (!makeOrderReq.demand_type.All(c => char.IsLetter(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "demand_type must be in Letter");
                }
                else if (!makeOrderReq.MembershipID.All(c => char.IsLetterOrDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "MembershipID must be in Letter and digit");
                }
                else if (!makeOrderReq.Vendor_code.All(c => char.IsLetterOrDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Vendor_code must be in Letter and digit");
                }
                else if (!makeOrderReq.item_code.All(c => char.IsLetterOrDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "item_code must be in Letter and digit");
                }
                else
                {
                    // check for vendor and member
                    IEnumerable<PnaMember> memberresults;
                    IEnumerable<PnaVendor> vendorresults;
                    IEnumerable<NightLifeProducts> NightLifeProductsresults;
                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where MembershipID=@MembershipID";

                        memberresults = await connlogon.QueryAsync<PnaMember>(selectQuerylogon, new
                        {
                            makeOrderReq.MembershipID
                        });
                        int rowlogon = memberresults.Count();
                        if (rowlogon >= 1)
                        {
                            // check for vendor
                            string selectQuerylogon2 = @"SELECT * FROM [DB_A57DC4_pnaDb_Vendor] where Vendor_code=@Vendor_code";

                            vendorresults = await connlogon.QueryAsync<PnaVendor>(selectQuerylogon2, new
                            {
                                makeOrderReq.Vendor_code
                            });
                            int rowlogon2 = vendorresults.Count();
                            if (rowlogon2 >= 1)
                            {
                                // check for item
                                string selectQuerylogon3 = @"SELECT * FROM [NightLifeProducts] where item_code=@item_code";

                                NightLifeProductsresults = await connlogon.QueryAsync<NightLifeProducts>(selectQuerylogon3, new
                                {
                                    makeOrderReq.item_code
                                });
                                int rowlogon3 = NightLifeProductsresults.Count();
                                if (rowlogon3 >= 1)
                                {
                                    // spool their info
                                    foreach (var memberinfo in memberresults)
                                    {
                                        foreach (var vendorinfo in vendorresults)
                                        {
                                            foreach (var iteminfo in NightLifeProductsresults)
                                            {
                                                //get discounted amount
                                                var getDiscounted_price = iteminfo.item_price - (iteminfo.item_price * iteminfo.item_discount / 100);

                                                var setproduct_itemto_order = new night_life_order()
                                                {
                                                    demand_type = makeOrderReq.demand_type,
                                                    item_price = iteminfo.item_price,
                                                    discounted_amount = getDiscounted_price,
                                                    item = iteminfo.item,
                                                    OrderId = OrderID,
                                                    MembershipID = makeOrderReq.MembershipID,
                                                    number_of_persons = makeOrderReq.number_of_persons,
                                                    IsItem_paid = false,
                                                    IsVendorCredited = false,
                                                    paymentMethod = "Wallet",
                                                    reservation_date = makeOrderReq.reservation_date,
                                                    service_category = makeOrderReq.service_category,
                                                    service_type = makeOrderReq.service_type,
                                                    table_type = makeOrderReq.table_type,
                                                    total_payable_amount = getDiscounted_price,
                                                    order_date = logdate,
                                                    Vendor_code = makeOrderReq.Vendor_code
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
                                                var OrderInsert = connlogon.Insert(setproduct_itemto_order);

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
                                                    MailText = MailText.Replace("[service_category]", makeOrderReq.service_category);
                                                    MailText = MailText.Replace("[totalpay]", getDiscounted_price.ToString("N", CultureInfo.CurrentCulture));
                                                    var SendEmailApi = new SendEmailApiReq()
                                                    {
                                                        htmlBody = MailText,
                                                        MailSubject = subject,
                                                        MailTo = memberinfo.Email
                                                    };

                                                    await _notificationServices.sendmail(SendEmailApi);
                                                    var setvendoremail_notification = new Orderto_pna_admin_emailreq()
                                                    {
                                                        item = iteminfo.item,
                                                        member_fnmae = memberinfo.FullName,
                                                        member_phone = memberinfo.PhoneNumbers,
                                                        orderdate = logdate.ToString(),
                                                        orderid = OrderID,
                                                        service_category = makeOrderReq.service_category,
                                                        totalpay = getDiscounted_price,
                                                        vendoremail = vendorinfo.Email,
                                                        vendorname = vendorinfo.companyName
                                                    };
                                                    //send mail to vendor
                                                    await Send_emailnotification_vendor(setvendoremail_notification);
                                                    var setpna_adminemail_notification = new Orderto_pna_admin_emailreq()
                                                    {
                                                        item = iteminfo.item,
                                                        member_fnmae = memberinfo.FullName,
                                                        member_phone = memberinfo.PhoneNumbers,
                                                        orderdate = logdate.ToString(),
                                                        orderid = OrderID,
                                                        service_category = makeOrderReq.service_category,
                                                        totalpay = getDiscounted_price,
                                                        vendoremail = vendorinfo.Email,
                                                        vendorname = vendorinfo.companyName
                                                    };
                                                    //send mail to Pna Admin
                                                    await Send_emailnotification_pnaadmin(setpna_adminemail_notification);
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "SUCCESS", setDTO);

                                                    var syslog = new Sys_logReq()
                                                    {
                                                        channel = _appChannel.Channel1,
                                                        description = $"Member {memberinfo.FullName} makes booking order_id:{OrderID} from vendor: {vendorinfo.companyName}",
                                                        UserID = memberinfo.Email,
                                                        Usertype = _appSystemType.Type7
                                                    };
                                                    var sys_log = await _systemUtility.sys_Log(syslog);
                                                }
                                                else
                                                {
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"could not process Order, try again");
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
                                               JsonConvert.SerializeObject(makeOrderReq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> Make_order_payment(Make_order_paymentReq make_Order_PaymentReqs)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            string Generatepaymentref = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 9);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            var logmonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
            try
            {
                if (string.IsNullOrWhiteSpace(make_Order_PaymentReqs.OrderId))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "OrderId is required");
                }
                else if (string.IsNullOrWhiteSpace(make_Order_PaymentReqs.TransactionPin))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "TransactionPin is required");
                }
                else if (string.IsNullOrWhiteSpace(make_Order_PaymentReqs.WalletId))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId is required");
                }
                else
                {
                    IEnumerable<night_life_order> NightLife_Ordersresults;
                    IEnumerable<PnaVendor> PnaVendorresults;
                    IEnumerable<PnaMember> memberresults;
                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [NightLifeOrders] where OrderId=@OrderId";

                        NightLife_Ordersresults = await connlogon.QueryAsync<night_life_order>(selectQuerylogon, new
                        {
                            make_Order_PaymentReqs.OrderId
                        });
                        int rowlogon = NightLife_Ordersresults.Count();
                        if (rowlogon >= 1)
                        {
                            foreach (var orderInfo in NightLife_Ordersresults)
                            {
                                string selectQuerylogon2 = @"SELECT * FROM [DB_A57DC4_pnaDb_Vendor] where Vendor_code=@Vendor_code";

                                PnaVendorresults = await connlogon.QueryAsync<PnaVendor>(selectQuerylogon2, new
                                {
                                    orderInfo.Vendor_code
                                });
                                string selectQuerylogon3 = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where MembershipID=@MembershipID";

                                memberresults = await connlogon.QueryAsync<PnaMember>(selectQuerylogon3, new
                                {
                                    orderInfo.MembershipID
                                });
                                foreach (var vendorInfo in PnaVendorresults)
                                {
                                    foreach (var memberInfo in memberresults)
                                    {
                                        var DebitWalletReq = new Debit_WalletReq()
                                        {
                                            WalletId = make_Order_PaymentReqs.WalletId,
                                            amount = make_Order_PaymentReqs.amount,
                                            TransactionPin = make_Order_PaymentReqs.TransactionPin
                                        };
                                        // call wallet debit service
                                        var debit_Wallet = await _memberService.Debit_Wallet(DebitWalletReq);
                                        var getcode = debit_Wallet.Code;
                                        if (getcode != "00")
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"{debit_Wallet.Message}");
                                        }
                                        else
                                        {
                                            var reqBody1 = JsonConvert.SerializeObject(debit_Wallet);
                                            var debit_WalletDTO = JsonConvert.DeserializeObject<debit_WalletDTOs>(reqBody1);
                                            //Send Debit sms to user
                                            string Drmsg = $"Your Wallet Account Has Been Debited with N{make_Order_PaymentReqs.amount.ToString("N", CultureInfo.CurrentCulture)} on {logdate.ToString("G", DateTimeFormatInfo.InvariantInfo)} By PNA for payment of {orderInfo.item} to {vendorInfo.companyName} Avail Bal: N{debit_WalletDTO.Data.Wallet_balance.ToString("N", CultureInfo.CurrentCulture)}";
                                            string json1 = File.ReadAllText(@"Resources\emp.json");
                                            dynamic jsonObj1 = Newtonsoft.Json.JsonConvert.DeserializeObject(json1);
                                            jsonObj1["SMS"]["message"]["messagetext"] = $"{Drmsg}";
                                            jsonObj1["SMS"]["recipients"]["gsm"][0]["msidn"] = $"{memberInfo.PhoneNumbers}";
                                            string output1 = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj1, Newtonsoft.Json.Formatting.Indented);

                                            File.WriteAllText(@"Resources\emp.json", output1);

                                            string endpoint1 = $"{_baseUrls.SMSAPI}";
                                            var reqBody6 = JsonConvert.SerializeObject(jsonObj1);
                                            var SmsResponse1 = await _httpClient.Post(reqBody6, endpoint1, requestId);
                                            //Log to system
                                            var syslog1 = new Sys_logReq()
                                            {
                                                channel = _appChannel.Channel1,
                                                description = $"{memberInfo.FullName} Wallet Account Has Been Debited With  N{DebitWalletReq.amount.ToString("N", CultureInfo.CurrentCulture)} on {logdate.ToString("G", DateTimeFormatInfo.InvariantInfo)}By PNA for payment of{orderInfo.item} to {vendorInfo.companyName}",
                                                UserID = memberInfo.Email,
                                                Usertype = _appSystemType.Type7
                                            };
                                            var sys_log1 = await _systemUtility.sys_Log(syslog1);
                                            //call monnify Disbursement endpoint
                                            var Debit_PNA = new Debit_PNAreq()
                                            {
                                                amount = DebitWalletReq.amount,
                                                destinationAccountNumber = vendorInfo.Accountnumber,
                                                destinationBankCode = vendorInfo.BankCode,
                                                narration = $"Order Payment for {orderInfo.item} by {memberInfo.FullName} from {vendorInfo.companyName} on Play Network Africa(PNA) App",
                                                reference = Generatepaymentref
                                            };
                                            var initiate_debit = await _memberService.debit_PNA(Debit_PNA);
                                            var reqBody2 = JsonConvert.SerializeObject(initiate_debit);
                                            var initiate_debitDTO = JsonConvert.DeserializeObject<Initiat_debit_PNA>(reqBody2);
                                            if (initiate_debitDTO.data.responseBody.status == "SUCCESS")
                                            {
                                                var id = orderInfo.Id;
                                                var NightLife_Orders = connlogon.Get<night_life_order>(id);
                                                NightLife_Orders.IsItem_paid = true;
                                                NightLife_Orders.IsVendorCredited = true;
                                                NightLife_Orders.payment_status = initiate_debitDTO.data.responseBody.status;
                                                NightLife_Orders.paymentReference = initiate_debitDTO.data.responseBody.reference;
                                                NightLife_Orders.payment_date = initiate_debitDTO.data.responseBody.dateCreated;
                                                var checkquery = connlogon.Update(NightLife_Orders);
                                                if (checkquery == true)
                                                {
                                                    //send reciept to user
                                                    // send Order reciept to email member
                                                    var subject = $"Your payment for Order {orderInfo.OrderId} - is successfull.";
                                                    string path1 = @"Resources\EmailTemplate\Order_payment_receipt.html";
                                                    string fullPath;
                                                    fullPath = Path.GetFullPath(path1);
                                                    StreamReader str = new StreamReader(fullPath);
                                                    string MailText = str.ReadToEnd();
                                                    str.Close();
                                                    MailText = MailText.Replace("[transaction_date]", initiate_debitDTO.data.responseBody.dateCreated.ToString());
                                                    MailText = MailText.Replace("[orderid]", orderInfo.OrderId);
                                                    MailText = MailText.Replace("[item]", orderInfo.item);
                                                    MailText = MailText.Replace("[paymentReference]", initiate_debitDTO.data.responseBody.reference);
                                                    MailText = MailText.Replace("[service_category]", orderInfo.service_category);
                                                    MailText = MailText.Replace("[totalpay]", orderInfo.discounted_amount.ToString("N", CultureInfo.CurrentCulture));
                                                    MailText = MailText.Replace("[amount]", orderInfo.discounted_amount.ToString("N", CultureInfo.CurrentCulture));
                                                    var SendEmailApi = new SendEmailApiReq()
                                                    {
                                                        htmlBody = MailText,
                                                        MailSubject = subject,
                                                        MailTo = memberInfo.Email
                                                    };

                                                    await _notificationServices.sendmail(SendEmailApi);
                                                    //send email to vendor
                                                    var subject2 = $"PNA | Payment Receipt For {vendorInfo.companyName}";
                                                    string path2 = @"Resources\EmailTemplate\Order_payment_receipt_Vendor.html";
                                                    string fullPath2;
                                                    fullPath2 = Path.GetFullPath(path2);
                                                    StreamReader str2 = new StreamReader(fullPath2);
                                                    string MailText2 = str2.ReadToEnd();
                                                    str2.Close();
                                                    MailText2 = MailText2.Replace("[orderid]", orderInfo.OrderId);
                                                    MailText2 = MailText2.Replace("[item]", orderInfo.item);
                                                    MailText2 = MailText2.Replace("[vendorname]", vendorInfo.companyName);
                                                    MailText2 = MailText2.Replace("[paymentReference]", initiate_debitDTO.data.responseBody.reference);
                                                    MailText2 = MailText2.Replace("[transaction_date]", initiate_debitDTO.data.responseBody.dateCreated.ToString());
                                                    MailText2 = MailText2.Replace("[amount]", orderInfo.discounted_amount.ToString("N", CultureInfo.CurrentCulture));
                                                    var SendEmailApi2 = new SendEmailApiReq()
                                                    {
                                                        htmlBody = MailText2,
                                                        MailSubject = subject2,
                                                        MailTo = vendorInfo.Email
                                                    };
                                                    await _notificationServices.sendmail(SendEmailApi2);
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, $"Payment successful");
                                                }
                                                else
                                                {
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"Payment Processing failed");
                                                }
                                            }
                                            else
                                            {
                                                var id = orderInfo.Id;
                                                var NightLife_Orders = connlogon.Get<night_life_order>(id);
                                                NightLife_Orders.IsItem_paid = false;
                                                NightLife_Orders.IsVendorCredited = false;
                                                NightLife_Orders.payment_status = initiate_debitDTO.data.responseBody.status;
                                                var checkquery = connlogon.Update(NightLife_Orders);
                                                // call reversal service
                                                var ReversalFundReq = new ReversalFundReq()
                                                {
                                                    amount = DebitWalletReq.amount,
                                                    TransactionPin = DebitWalletReq.TransactionPin,
                                                    WalletId = DebitWalletReq.WalletId
                                                };
                                                var reversal_Wallet = await _memberService.reversal_Wallet(ReversalFundReq);
                                                if (reversal_Wallet.Code != "00")
                                                {
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"{reversal_Wallet.Message}");
                                                }
                                                else
                                                {
                                                    var reqBody3 = JsonConvert.SerializeObject(reversal_Wallet);
                                                    var reversal_WalletDTO = JsonConvert.DeserializeObject<reversal_WalletDTOs>(reqBody1);
                                                    //sending SMS to User
                                                    string Crmsg = $"Your Wallet Account Has Been Credited With  N{DebitWalletReq.amount.ToString("N", CultureInfo.CurrentCulture)} on {logdate.ToString("G", DateTimeFormatInfo.InvariantInfo)} By PNA due to failure for payment of{orderInfo.item} to {vendorInfo.companyName} Avail Bal: N{reversal_WalletDTO.data.wallet_balance.ToString("N", CultureInfo.CurrentCulture)}";
                                                    //var Drsms = await _notificationServices.SendSMS(authinfo.PhoneNumber,Drmsg);
                                                    string json = File.ReadAllText(@"Resources\emp.json");
                                                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                                                    jsonObj["SMS"]["message"]["messagetext"] = $"{Crmsg}";
                                                    jsonObj["SMS"]["recipients"]["gsm"][0]["msidn"] = $"{memberInfo.PhoneNumbers}";
                                                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                                                    File.WriteAllText(@"Resources\emp.json", output);

                                                    string endpoint = $"{_baseUrls.SMSAPI}";
                                                    var reqBody = JsonConvert.SerializeObject(jsonObj);
                                                    var SmsResponse = await _httpClient.Post(reqBody, endpoint, requestId);

                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"Sorry Payment Processing has failed and your money has been reversed to your wallet");
                                                    //Log to system
                                                    var syslog = new Sys_logReq()
                                                    {
                                                        channel = _appChannel.Channel1,
                                                        description = $"{memberInfo.FullName} Wallet Account Has Been Credited With  N{DebitWalletReq.amount.ToString("N", CultureInfo.CurrentCulture)} on {logdate.ToString("G", DateTimeFormatInfo.InvariantInfo)} By PNA due to failure for payment of{orderInfo.item} to {vendorInfo.companyName}",
                                                        UserID = memberInfo.Email,
                                                        Usertype = _appSystemType.Type7
                                                    };
                                                    var sys_log = await _systemUtility.sys_Log(syslog);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "No such Order");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[PNAAPI][Make_order_payment][Response] => {ex.Message} | [request]=> " +
                                               JsonConvert.SerializeObject(make_Order_PaymentReqs) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> Send_emailnotification_pnaadmin(Orderto_pna_admin_emailreq orderto_Pna_Admin_Emailreq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                // send Order reciept to email member
                var subject = $"New {orderto_Pna_Admin_Emailreq.member_fnmae} Order {orderto_Pna_Admin_Emailreq.orderid} - has been requested.";
                string path1 = @"Resources\EmailTemplate\Ordertopna_admin_notification.html";
                string fullPath;
                fullPath = Path.GetFullPath(path1);
                StreamReader str = new StreamReader(fullPath);
                string MailText = str.ReadToEnd();
                str.Close();
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
                    MailTo = "services@playnetworkafricaservices.com"
                };

                await _notificationServices.sendmail(SendEmailApi);
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[PNAAPI][Send_emailnotification_pnaadmin][Response] => {ex.Message} | [request]=> " +
                                               JsonConvert.SerializeObject(orderto_Pna_Admin_Emailreq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> Send_emailnotification_vendor(Orderto_pna_admin_emailreq orderto_Pna_Admin_Emailreq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                // send Order reciept to email member
                var subject = $"New {orderto_Pna_Admin_Emailreq.member_fnmae} Order {orderto_Pna_Admin_Emailreq.orderid} - has been requested.";
                string path1 = @"Resources\EmailTemplate\Ordertovendor_notification.html";
                string fullPath;
                fullPath = Path.GetFullPath(path1);
                StreamReader str = new StreamReader(fullPath);
                string MailText = str.ReadToEnd();
                str.Close();
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
                    MailTo = orderto_Pna_Admin_Emailreq.vendoremail
                };

                await _notificationServices.sendmail(SendEmailApi);
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[PNAAPI][Send_emailnotification_vendor][Response] => {ex.Message} | [request]=> " +
                                               JsonConvert.SerializeObject(orderto_Pna_Admin_Emailreq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }
    }
}