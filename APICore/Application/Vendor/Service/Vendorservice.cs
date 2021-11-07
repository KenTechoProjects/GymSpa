using APICore.Application.Member.Interface;
using APICore.Application.Vendor.Interface;
using AppCore.Application.TransactionNotification;
using Dapper;
using Dapper.Contrib.Extensions;
using Domain.Application.SystemUtility.Sys_Logs;
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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;
using Utilities.Interface;
using Utilities.SystemActivity.Interface;

namespace APICore.Application.Vendor.Service
{
   public  class Vendorservice : Ivendorservice
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
        public Vendorservice(Logger logger, IOptions<AppKeys> appKeys, IConfiguration configuration, ISystemActivityService systemUtility, IOptions<AppSystemType> appsystemtype, IOptions<AppChannel> channel, ISharedService sharedService, INotificationServices notificationServices, IOptions<BaseUrls> baseUrls, UIHttpClient httpClient, ITransactionNotification transactionNotification, IMemberService memberService)
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

        public async Task<ResponseParam> Create_nightlife_product(Create_nightlife_productReq create_Nightlife_ProductReq)
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
            string itemcode = Helper.GenerateUniqueId(8);
            Random Rnd = new Random();
            string ActivationCode = (Rnd.Next(100000, 999999)).ToString();
            try
            {
                if (string.IsNullOrWhiteSpace(create_Nightlife_ProductReq.item))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Item Name is Required");
                }else if (string.IsNullOrWhiteSpace(create_Nightlife_ProductReq.vendorcode))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Vendor code is Required");
                }
                else if (Path.GetExtension(create_Nightlife_ProductReq.item_image.FileName).ToLower() != ".jpg"
              && Path.GetExtension(create_Nightlife_ProductReq.item_image.FileName).ToLower() != ".png"
              && Path.GetExtension(create_Nightlife_ProductReq.item_image.FileName).ToLower() != ".jpeg")
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Item Image  is not a valid image file type");
                }
                else
                {
                    IEnumerable<PnaVendor> vendorresults;
                    using (IDbConnection connlogon = Connection)
                    {
                        // check for vendor
                        string selectQuerylogon2 = @"SELECT * FROM [DB_A57DC4_pnaDb_Vendor] where Vendor_code=@vendorcode";

                        vendorresults = await connlogon.QueryAsync<PnaVendor>(selectQuerylogon2, new
                        {
                            create_Nightlife_ProductReq.vendorcode
                        });
                        int rowlogon2 = vendorresults.Count();
                        if (rowlogon2 >= 1)
                        {
                            foreach (var vendorinfo in vendorresults)
                            {
                                var folderName = Path.Combine("Resources", "Images");
                                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                                var fileName = ContentDispositionHeaderValue.Parse(create_Nightlife_ProductReq.item_image.ContentDisposition).FileName.Trim('"');
                                //var fileName = profile_pic.FileName;
                                var fullPath = Path.Combine(pathToSave, fileName);
                                var dbPath = Path.Combine(folderName, fileName);

                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    create_Nightlife_ProductReq.item_image.CopyTo(stream);

                                }

                                byte[] imageArray = System.IO.File.ReadAllBytes($"{fullPath}");
                                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                                var getprofilepicUrl = _baseUrls.ProfilepictureUrl + fileName;

                                var insert_product = new NightLifeProducts()
                                {
                                     item_image= getprofilepicUrl,
                                      date_created=logdate,
                                       item=create_Nightlife_ProductReq.item,
                                        item_code=itemcode,
                                         item_discount=create_Nightlife_ProductReq.item_discount,
                                          item_price=create_Nightlife_ProductReq.item_price,
                                           Vendor_code=create_Nightlife_ProductReq.vendorcode
                                };
                                var Item_Insert = connlogon.Insert(insert_product);
                                if (Item_Insert >= 1)
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Successfully created new product");
                                    var syslog = new Sys_logReq()
                                    {
                                        channel = _appChannel.Channel2,
                                        description = $"Vendor {vendorinfo.companyName} created new product item {create_Nightlife_ProductReq.item}",
                                        UserID = vendorinfo.Email,
                                        Usertype = _appSystemType.Type10
                                    };
                                    var sys_log = await _systemUtility.sys_Log(syslog);
                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Could not create Item, kindly try again");
                                }


                            }

                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "No such Vendor");
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[PNAAPI][member new Order][Response] => {ex.Message} | [request]=> " +
                                                              JsonConvert.SerializeObject(create_Nightlife_ProductReq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }
    }
}
