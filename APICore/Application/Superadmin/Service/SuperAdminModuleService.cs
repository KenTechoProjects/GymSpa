using APICore.Application.Superadmin.Interface;
using AppCore.Application.TransactionNotification;
using Dapper;
using Dapper.Contrib.Extensions;
using Domain.Application.SuperAdmin;
using Domain.Application.SuperAdmin.AuthDTOs;
using Domain.Application.SuperAdmin.Login;
using Domain.Application.SuperMember.DTO;
using Domain.Application.SystemUtility.Sys_Logs;
using Domain.Application.Vendor;
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
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;
using Utilities.Interface;
using Utilities.SystemActivity.Interface;

namespace APICore.Application.Superadmin.Service
{
    public class SuperAdminModuleService : ISuperAdminModuleService
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
        public SuperAdminModuleService(Logger logger, IOptions<AppKeys> appKeys, IConfiguration configuration, ISystemActivityService systemUtility, IOptions<AppSystemType> appsystemtype, IOptions<AppChannel> channel, ISharedService sharedService, INotificationServices notificationServices, IOptions<BaseUrls> baseUrls, UIHttpClient httpClient, ITransactionNotification transactionNotification)
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
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DB_A57DC4_PNADb"));
            }
        }

        public async Task<ResponseParam> create_vendor(VendorRegReq vendorRegReq)
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
            string VendorCode = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 5);
            string WalletId = Helper.GenerateUniqueId(6);
            Random Rnd = new Random();
            string ActivationCode = (Rnd.Next(100000, 999999)).ToString();
            string checkphonenumber = vendorRegReq.Phonenumber;
            string[] checkphionenumberarray = { "0803", "081", "0703", "0706", "0706", "0806", "0810", "0813", "0814", "0816", "0903", "0906", "0705", "0805", "0807", "0811", "0815", "0905", "0701", "0802", "0708", "0808", "0812", "0907", "0902", "0809", "0901", "0817", "0818", "0909" };

            try
            {
                //validation
                Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
                bool isValidEmail = regex.IsMatch(vendorRegReq.Email);
                if (string.IsNullOrWhiteSpace(vendorRegReq.Accountname) || string.IsNullOrWhiteSpace(vendorRegReq.Accountnumber) || string.IsNullOrWhiteSpace(vendorRegReq.BankName) || string.IsNullOrWhiteSpace(vendorRegReq.Email) || string.IsNullOrWhiteSpace(vendorRegReq.company_Address) || string.IsNullOrWhiteSpace(vendorRegReq.Line_of_Business) || string.IsNullOrWhiteSpace(vendorRegReq.Name_of_Company) || string.IsNullOrWhiteSpace(vendorRegReq.RC_Number) || string.IsNullOrWhiteSpace(vendorRegReq.Services_offered) || string.IsNullOrWhiteSpace(vendorRegReq.TIN_number))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "All Fields are required");

                }
                else if (vendorRegReq.Phonenumber.Length > 11)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "PhoneNumber must not be more than 11 digits");
                }
                else if (vendorRegReq.Phonenumber.Length < 11)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "PhoneNumber must not be less than 11 digits");
                }
                else if (!isValidEmail)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "The email is invalid");
                }
                else if (!checkphionenumberarray.Any(checkphonenumber.Contains))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Not a valid PhoneNumber format");
                }
                else if (!vendorRegReq.Email.Any(char.IsLower))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Email must be in Lowercase");
                }
                else if (!checkphonenumber.All(c => char.IsDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "PhoneNumber must be number");
                }
                else if (!vendorRegReq.Accountnumber.All(c => char.IsDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Accountnumber must be a number");
                }
                else if (string.IsNullOrWhiteSpace(vendorRegReq.BankCode))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "BankCode");
                }else if (!vendorRegReq.BankCode.All(c=>char.IsDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "BankCode must be a number");
                }
                else
                {
                    //Log Api Request
                    await _systemUtility.JsonRequestLog(vendorRegReq);
                  
                    var Password = Cipher.GenerateHash(Generatepassword);
                    using (IDbConnection connu = Connection)
                    {
                        //Check if user already exist
                        string selectQuery = @"SELECT * FROM [DB_A57DC4_pnaDb_Vendor] where RCnumber=@RC_Number or companyName=@Name_of_Company";

                        var userExist = await connu.QueryAsync(selectQuery, new
                        {
                           vendorRegReq.RC_Number,
                           vendorRegReq.Name_of_Company
                        });
                        int row = userExist.Count();
                        if (row >= 1)
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Vendor is already registered");
                        }
                        else
                        {
                            var SetPnaVendor = new PnaVendor()
                            {
                                Accountnumber=vendorRegReq.Accountnumber,
                                 BankName=vendorRegReq.BankName,
                                  Businesscategory=vendorRegReq.Services_offered,
                                   Companyaddress=vendorRegReq.company_Address,
                                    companyName=vendorRegReq.Name_of_Company,
                                      Email=vendorRegReq.Email,
                                       RCnumber=vendorRegReq.RC_Number,
                                        TaxID= vendorRegReq.TIN_number,
                                         Vendor_code=VendorCode,
                                           Accountname=vendorRegReq.Accountname,
                                            PhoneNumber=vendorRegReq.Phonenumber,
                                             Account_creation_date=logdate,
                                              BankCode=vendorRegReq.BankCode

                            };

                            var setvendorAuth = new PnaVendorAuth()
                            {
                                 PhoneNumber=vendorRegReq.Phonenumber,
                                  Email=vendorRegReq.Email,
                                   Password=Password

                            };
                            // insert to vendor table
                            var vendorinsert = connu.Insert(SetPnaVendor);
                            //insert to vendor auth table
                            connu.Insert(setvendorAuth);
                            if (vendorinsert>=1)
                            {
                                ////send activation code
                                string Message = $"Hello your company account has been created on Play Network Africa (PNA) here is your Vendor code:{VendorCode}, kindly check this {vendorRegReq.Email} for other instruction";
                                string json = File.ReadAllText(@"Resources\emp.json");
                                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                                jsonObj["SMS"]["message"]["messagetext"] = $"{Message}";
                                jsonObj["SMS"]["recipients"]["gsm"][0]["msidn"] = $"{vendorRegReq.Phonenumber}";
                                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                                File.WriteAllText(@"Resources\emp.json", output);

                                string endpoint = $"{_baseUrls.SMSAPI}";
                                var reqBody = JsonConvert.SerializeObject(jsonObj);
                                var SmsResponse = await _httpClient.Post(reqBody, endpoint, requestId);

                                var SmsDTO = JsonConvert.DeserializeObject<SmsResponse>(SmsResponse);
                                //send email
                                var subject = "Welcome to Play Network Africa (PNA)";
                                string path1 = @"Resources\EmailTemplate\AdminWelcome.html";
                                string fullPath;
                                fullPath = Path.GetFullPath(path1);
                                StreamReader str = new StreamReader(fullPath);
                                string MailText = str.ReadToEnd();
                                str.Close();
                                MailText = MailText.Replace("[fname]", vendorRegReq.Name_of_Company.ToUpper());
                                MailText = MailText.Replace("[email]", vendorRegReq.Email);
                                MailText = MailText.Replace("[password]", Generatepassword);
                                MailText = MailText.Replace("[vendor_code]", VendorCode);

                                var SendEmailApi = new SendEmailApiReq()
                                {
                                    htmlBody = MailText,
                                    MailSubject = subject,
                                    MailTo = vendorRegReq.Email
                                };

                                await _notificationServices.sendmail(SendEmailApi);
                                if (SmsDTO.response.status == "SUCCESS")
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "SUCCESS");

                                    var syslog = new Sys_logReq()
                                    {
                                        channel = _appChannel.Channel2,
                                        description = $"Created New Vendor {vendorRegReq.Name_of_Company}",
                                        UserID = vendorRegReq.Email,
                                        Usertype = _appSystemType.Type10
                                    };
                                    var sys_log = await _systemUtility.sys_Log(syslog);

                                    
                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"could not send message");
                                }
                            }
                            else
                            {
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"could not process new vendor entry, try again");
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][Create new member][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][Admin Create new Vendor][Response] => {ex.Message} | [request]=> " +
                                                JsonConvert.SerializeObject(vendorRegReq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> GetSuperAdminProfileByLoginToken(string Logintoken)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrWhiteSpace(Logintoken))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "LoginToken is required");
                }
                if (Logintoken.Length <= 32)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Not a valid LoginToken");
                }
                IEnumerable<SuperAdminLoginResponse> userresults;
                IEnumerable<SuperAdminAuthResponse> userauthresults;
                using (IDbConnection connlogon = Connection)
                {
                    string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_SuperAdmin] where Token=@Logintoken";

                    userauthresults = await connlogon.QueryAsync<SuperAdminAuthResponse>(selectQuerylogon, new
                    {
                        Logintoken
                    });
                    int rowlogon = userauthresults.Count();
                    // log into database

                    if (rowlogon >= 1)
                    {
                        foreach (var authinfo in userauthresults)
                        {

                            if (DateTime.Now > authinfo.TokenExpiry)
                            {

                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Login Token has expired.");

                            }
                            else
                            {
                                //Check for user
                                using (IDbConnection conn = Connection)
                                {
                                    string selectQuery = @"SELECT * FROM [DB_A57DC4_pnaDb_SuperAdmin] where username=@username";

                                    userresults = await conn.QueryAsync<SuperAdminLoginResponse>(selectQuery, new
                                    {
                                        authinfo.username
                                    });
                                    int row = userresults.Count();
                                    if (row >= 1)
                                    {
                                        foreach (var userauthinfo in userresults)
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "User Info", userauthinfo);
                                        }
                                    }
                                    else
                                    {

                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "No such User");
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid Token");

                    }

                }

            }
            catch (Exception ex)
            {

                _logger.LogError($"[PNA][GetSuperAdminProfile][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> ProfileSuperAdmin(SuperAdminReq superAdminReq)
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

            try
            {
                if (string.IsNullOrWhiteSpace(superAdminReq.username) || string.IsNullOrWhiteSpace(superAdminReq.password))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "UserName or Password is Required");
                }
                if (superAdminReq.TokenKey != _appKeys.TokenKey)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Your are not Authorize to profile PNA SuperAdmin");
                }
                if (superAdminReq.password.Length <= 8)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Password must not be less than 8 character");
                }
                // create superAdmin
                var Password = Cipher.GenerateHash(superAdminReq.password);
                var user_type = "SuperAdmin";
                var Status = "Active";
                int roleid = 0;
                var role = "SuperAdmin";
                var creationdate = logdate;
                using (IDbConnection conn = Connection)
                {

                    string sqlQuery = "INSERT INTO [DB_A57DC4_pnaDb_SuperAdmin]([username],[password],[user_type],[status],[creationdate],[roleid],[role],[last_login],[TokenExpiry],[last_activity]) VALUES(@username,@Password,@user_type,@Status,@creationdate,@roleid,@role,@a,@b,@c)";
                    var results = await conn.ExecuteAsync(sqlQuery,
                         new
                         {
                             superAdminReq.username,
                             Password,
                             user_type,
                             Status,
                             creationdate,
                             roleid,
                             role,
                             a = logdate,
                             b = logdate,
                             c = logdate
                         });

                    if (results >= 1)
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Success");
                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Failed Try again in a moment");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][CreateSuper Admin][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> SuperAdmincreatesSuperMember(SuperMemberRegReq superMemberRegReq)
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
            string WalletId = Helper.GenerateUniqueId(6);
            double ReferralLimit = 1000;
            double WalletBalance = 0;

            try
            {
                // check for SuperADMIN permission
                using (IDbConnection conn = Connection)
                {
                    string selectQuery = @"SELECT * FROM [DB_A57DC4_pnaDb_SuperAdmin] where SoftToken=@AdminSoftToken and Id=@AdminUserID";

                    var Superadminresults = await conn.QueryAsync(selectQuery, new
                    {
                        superMemberRegReq.AdminSoftToken,
                        superMemberRegReq.AdminUserID
                    });
                    int row = Superadminresults.Count();
                    if (row >= 1)
                    {
                        // operation actions

                        // check for user_type
                        IEnumerable<UserTypeResponse> usertyperesults;
                        using (IDbConnection connlogon = Connection)
                        {
                            string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Sys_Roles] where RoleName=@user_type";

                            usertyperesults = await connlogon.QueryAsync<UserTypeResponse>(selectQuerylogon, new
                            {
                                superMemberRegReq.user_type

                            });
                            int rowlogon = usertyperesults.Count();
                            // log into database

                            if (rowlogon >= 1)
                            {
                                foreach (var usertypeInfo in usertyperesults)
                                {


                                    // create superAdmin
                                    var Password = Cipher.GenerateHash(Generatepassword);
                                    //var Status = "Active";
                                    int roleid = usertypeInfo.Id;
                                    var role = usertypeInfo.RoleName;
                                    var CreatedBy = _appSystemType.Type1;
                                    var creationdate = logdate;
                                    using (IDbConnection connin = Connection)
                                    {

                                        string selectQuerym = @"SELECT * FROM [DB_A57DC4_pnaDb_SuperMember] where PhoneNumber=@PhoneNumber or Email=@Email";

                                        var userExist = await conn.QueryAsync(selectQuerym, new
                                        {
                                            superMemberRegReq.PhoneNumber,
                                            superMemberRegReq.Email
                                        });
                                        int rowm = userExist.Count();
                                        if (rowm >= 1)
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "user already registered");
                                        }
                                        else
                                        {

                                            string sqlQuery = "INSERT INTO [DB_A57DC4_pnaDb_SuperMember]([FullName],[PhoneNumber],[Email],[ReferralCode],[WalletId],[ReferralLimit],[AccountCreateDate]) VALUES(@FullName,@PhoneNumber,@Email,@ReferralCode,@WalletId,@ReferralLimit,@a)";
                                            var results = await connin.ExecuteAsync(sqlQuery,
                                                 new
                                                 {
                                                     superMemberRegReq.FullName,
                                                     superMemberRegReq.PhoneNumber,
                                                     superMemberRegReq.Email,
                                                     ReferralCode,
                                                     WalletId,
                                                     ReferralLimit,
                                                     a = logdate

                                                 });
                                            string sqlQuery2 = "INSERT INTO [DB_A57DC4_pnaDb_Wallet]([WalletId],[WalletBalance]) VALUES(@WalletId,@WalletBalance)";
                                            var results2 = await connin.ExecuteAsync(sqlQuery2,
                                                 new
                                                 {
                                                     WalletId,
                                                     WalletBalance
                                                 });
                                            string sqlQuery3 = "INSERT INTO [DB_A57DC4_pnaDb_SuperMemberAuth]([PhoneNumber],[Email],[Password],[TokenExpiry]) VALUES(@PhoneNumber,@Email,@Password,@a)";
                                            var results3 = await connin.ExecuteAsync(sqlQuery3,
                                                 new
                                                 {
                                                     superMemberRegReq.PhoneNumber,
                                                     superMemberRegReq.Email,
                                                     Password,
                                                     a=logdate
                                                 });
                                            if (results >= 1)
                                            {
                                                var SuperMemberDetails = new SuperMemberDetails()
                                                {
                                                     FullName= superMemberRegReq.FullName,
                                                      Email= superMemberRegReq.Email,
                                                       PhoneNumber=superMemberRegReq.PhoneNumber,
                                                        Password=Generatepassword,
                                                         ReferralCode=ReferralCode,
                                                          WalletId=WalletId
                                                };
                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Your Logon Details, Kindly login and change your Login Details", SuperMemberDetails);
                                                // Send Email Notification to New Admin Staff
                                                //end
                                                // Log Activities
                                                var syslog = new Sys_logReq()
                                                {
                                                    channel = _appChannel.Channel2,
                                                    description = $"Created New SuperMember {superMemberRegReq.FullName} {usertypeInfo.RoleName}",
                                                    UserID = superMemberRegReq.AdminUserID.ToString(),
                                                    Usertype = _appSystemType.Type1
                                                };
                                                var sys_log = await _systemUtility.sys_Log(syslog);
                                            }
                                            else
                                            {
                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Failed Try again in a moment");
                                            }

                                        }

                                        

                                    }

                                }
                            }
                            else
                            {
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid UserType");

                            }

                        }

                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Oops. No authorization to Onboard Admin Staffs");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][CreateStaff Admin][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> SuperAdmincreatestaff(SuperAdminCreateStaffReq superAdminCreateStaffReq)
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
            string Generateusername = Guid.NewGuid().ToString("N").ToUpper().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            try
            {
                // check for SuperADMIN permission
                using (IDbConnection conn = Connection)
                {
                    string selectQuery = @"SELECT * FROM [DB_A57DC4_pnaDb_SuperAdmin] where SoftToken=@AdminSoftToken and Id=@AdminUserID";

                    var Superadminresults = await conn.QueryAsync(selectQuery, new
                    {
                        superAdminCreateStaffReq.AdminSoftToken,
                        superAdminCreateStaffReq.AdminUserID
                    });
                    int row = Superadminresults.Count();
                    if (row >= 1)
                    {
                        // operation actions

                        if (superAdminCreateStaffReq.user_type != _appSystemType.Type1 || superAdminCreateStaffReq.user_type != _appSystemType.Type2 || superAdminCreateStaffReq.user_type != _appSystemType.Type3 || superAdminCreateStaffReq.user_type != _appSystemType.Type4 || superAdminCreateStaffReq.user_type != _appSystemType.Type5 || superAdminCreateStaffReq.user_type != _appSystemType.Type6 || superAdminCreateStaffReq.user_type != _appSystemType.Type7 || superAdminCreateStaffReq.user_type != _appSystemType.Type8 || superAdminCreateStaffReq.user_type != _appSystemType.Type9 || superAdminCreateStaffReq.user_type != _appSystemType.Type10)
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "New Admin Role Not Recognize by the system, kindly retry");
                        }
                        // check for user_type
                        IEnumerable<UserTypeResponse> usertyperesults;
                        using (IDbConnection connlogon = Connection)
                        {
                            string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Sys_Roles] where RoleName=@user_type";

                            usertyperesults = await connlogon.QueryAsync<UserTypeResponse>(selectQuerylogon, new
                            {
                                superAdminCreateStaffReq.user_type

                            });
                            int rowlogon = usertyperesults.Count();
                            // log into database

                            if (rowlogon >= 1)
                            {
                                foreach (var usertypeInfo in usertyperesults)
                                {


                                    // create superAdmin
                                    var Password = Cipher.GenerateHash(Generatepassword);
                                    var Status = "Active";
                                    int roleid = usertypeInfo.Id;
                                    var role = usertypeInfo.RoleName;
                                    var CreatedBy = _appSystemType.Type1;
                                    var creationdate = logdate;
                                    using (IDbConnection connin = Connection)
                                    {

                                        string sqlQuery = "INSERT INTO [DB_A57DC4_pnaDb_Admins]([username],[fullname],[password],[user_type],[status],[creationdate],[role],[last_login],[TokenExpiry],[last_activity],[CreatedBy]) VALUES(@Generateusername,@fullname,@Password,@RoleName,@Status,@creationdate,@role,@a,@b,@c,@CreatedBy)";
                                        var results = await connin.ExecuteAsync(sqlQuery,
                                             new
                                             {
                                                 Generateusername,
                                                 superAdminCreateStaffReq.fullname,
                                                 Password,
                                                 usertypeInfo.RoleName,
                                                 Status,
                                                 creationdate,
                                                
                                                 role,
                                                 
                                                 a = logdate,
                                                 b = logdate,
                                                 c = logdate,
                                                 CreatedBy
                                             });

                                        if (results >= 1)
                                        {
                                            var Admindetails = new AdminStaffResponse()
                                            {
                                                username = Generateusername,
                                                fullname = superAdminCreateStaffReq.fullname,
                                                user_type = usertypeInfo.RoleName,
                                                password = Generatepassword
                                            };

                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Your Logon Details, Kindly login and change your Login Details", Admindetails);
                                            // Send Email Notification to New Admin Staff
                                            //end
                                            // Log Activities
                                            var syslog = new Sys_logReq()
                                            {
                                                channel = _appChannel.Channel2,
                                                description = $"Created New Admin Staff {Generateusername} {usertypeInfo.RoleName}",
                                                UserID = superAdminCreateStaffReq.AdminUserID.ToString(),
                                                Usertype = _appSystemType.Type1
                                            };
                                            var sys_log = await _systemUtility.sys_Log(syslog);
                                        }
                                        else
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Failed Try again in a moment");
                                        }

                                    }

                                }
                            }
                            else
                            {
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid UserType");

                            }

                        }

                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Oops. No authorization to Onboard Admin Staffs");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][CreateStaff Admin][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> SuperAdmindelstaff(string superAdminSoftToken, int superAdminUserID, int AdminstaffID)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrWhiteSpace(superAdminSoftToken))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "superAdminSoftToken is Required");
                }
                // check for superAdmin
                using (IDbConnection conn = Connection)
                {
                    string selectQuery = @"SELECT * FROM [DB_A57DC4_pnaDb_SuperAdmin] where SoftToken=@superAdminSoftToken and Id=@superAdminUserID";

                    var Superadminresults = await conn.QueryAsync(selectQuery, new
                    {
                        superAdminSoftToken,
                        superAdminUserID
                    });
                    int row = Superadminresults.Count();
                    if (row >= 1)
                    {
                        // operation actions

                        // check for Admin staff 
                        // check for user_type
                        IEnumerable<AdminStaffResponse> adminstaffresults;
                        using (IDbConnection connlogon = Connection)
                        {
                            string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Admins] where Id=@AdminstaffID";

                            adminstaffresults = await connlogon.QueryAsync<AdminStaffResponse>(selectQuerylogon, new
                            {
                                AdminstaffID

                            });
                            int rowlogon = adminstaffresults.Count();
                            // log into database

                            if (rowlogon >= 1)
                            {
                                foreach (var adminstaffInfo in adminstaffresults)
                                {

                                    using (IDbConnection conndel = Connection)
                                    {
                                        string selectQuerydel = @"DELETE [DB_A57DC4_pnaDb_Admins] where Id=@AdminstaffID";

                                        var deladminstaffresults = await conndel.ExecuteAsync(selectQuerydel, new
                                        {
                                            AdminstaffID
                                        });
                                        // int rowdel = deladminstaffresults.Count();
                                        if (deladminstaffresults >= 1)
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Admin Staff Deleted Successfully");
                                            // Log Activities
                                            var syslog = new Sys_logReq()
                                            {
                                                channel = _appChannel.Channel2,
                                                description = $"Deleted Admin Staff {adminstaffInfo.fullname} {adminstaffInfo.username } {adminstaffInfo.user_type}",
                                                UserID = superAdminUserID.ToString(),
                                                Usertype = _appSystemType.Type1
                                            };
                                            var sys_log = await _systemUtility.sys_Log(syslog);
                                        }
                                        else
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Oops. Something went wrong. Please try again later");
                                        }

                                    }
                                }
                            }
                            else
                            {
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid Admin Staff");

                            }

                        }

                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Oops. No authorization to Delete Admin Staffs");
                    }
                }

            }
            catch (Exception ex)
            {

                _logger.LogError($"[PNA][ Super Admin delete staff][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> SuperAdminLogin(LoginReq loginReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrWhiteSpace(loginReq.password) || string.IsNullOrWhiteSpace(loginReq.username))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "UserName or Password is Required");
                }
                if (loginReq.password.Length <= 8)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Password must not be less than 8 character");
                }
                //Hash Password
                var Password = Cipher.GenerateHash(loginReq.password);
                // check for SuperAdmin User 
                IEnumerable<PnaSuperAdmin> loginRequestresults;
                using (IDbConnection connlogon = Connection)
                {
                    string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_SuperAdmin] where username=@username and password=@Password";

                    loginRequestresults = await connlogon.QueryAsync<PnaSuperAdmin>(selectQuerylogon, new
                    {
                        loginReq.username,
                        Password
                    });
                    int rowlogon = loginRequestresults.Count();
                    // log into database

                    if (rowlogon >= 1)
                    {
                        foreach (var loginInfo in loginRequestresults)
                        {
                            //cast SuperAdmin Id
                            // var SetsuperAdminID = new SuperAdminGetID()
                            //{
                            //Id= loginInfo.Id
                            //};

                            // Log Activities
                            var syslog = new Sys_logReq()
                            {
                                channel = _appChannel.Channel2,
                                description = $"Login Successful {loginReq.username}",
                                UserID = loginInfo.Id.ToString(),
                                Usertype = _appSystemType.Type1
                            };
                            // Generate Token

                            string token = Guid.NewGuid().ToString("N");
                            //update Auth Table 
                            var TokenExpiry = DateTime.Now.AddMinutes(5);
                            string setsofttoken = loginReq.username + Helper.GenerateUniqueId(3);
                            var SoftToken = Cipher.GenerateHash(setsofttoken);
                            using (IDbConnection conn = Connection)
                            {

                                string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_SuperAdmin] SET Token = @token,TokenExpiry=@TokenExpiry,SoftToken=@SoftToken WHERE username = @username";

                                var updateRequest = await conn.ExecuteAsync(updateQuery, new
                                {

                                    token,
                                    TokenExpiry,
                                    SoftToken,
                                    loginReq.username

                                });

                                if (updateRequest >= 1)
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Token", token);
                                    var sys_log = await _systemUtility.sys_Log(syslog);

                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Unable to Generate Token");
                                }
                            }
                        }
                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid  Username or Password, Kindly Retry");
                        // Log Activities
                        var syslog = new Sys_logReq()
                        {
                            channel = _appChannel.Channel2,
                            description = $"Login Failed {loginReq.username}",
                            UserID = "1",
                            Usertype = _appSystemType.Type1
                        };
                        var sys_log = await _systemUtility.sys_Log(syslog);
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][Login Super Admin][Response] => {ex.Message} | [requestId]=> {requestId}");

            }
            return response;
        }

        public async Task<ResponseParam> SuperAdminUpdateProfile(SuperAdminUpdateProfileReq superAdminUpdateProfileReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                // const int ImageMaximumBytes = 1024;
                if (string.IsNullOrEmpty(superAdminUpdateProfileReq.password) && string.IsNullOrEmpty(superAdminUpdateProfileReq.confirmpassword))
                {
                    //if (superAdminUpdateProfileReq.profile_pic.Length >= ImageMaximumBytes)
                    //{
                    //    response = new ResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Profile Picture size too much,kindly upload smaller size picture");
                    //}
                    if (Path.GetExtension(superAdminUpdateProfileReq.profile_pic.FileName).ToLower() != ".jpg"
                && Path.GetExtension(superAdminUpdateProfileReq.profile_pic.FileName).ToLower() != ".png"
                && Path.GetExtension(superAdminUpdateProfileReq.profile_pic.FileName).ToLower() != ".jpeg")
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Profile Picture is not a valid image file type");
                    }
                    //admiin picture
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    if (superAdminUpdateProfileReq.profile_pic.FileName.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(superAdminUpdateProfileReq.profile_pic.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            superAdminUpdateProfileReq.profile_pic.CopyTo(stream);
                        }

                        //return Ok(new { dbPath });

                        //update database
                        using (IDbConnection conn = Connection)
                        {

                            string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_SuperAdmin] SET username = @username,fullname=@fullname,phonenumber=@phonenumber,Email=@Email,profile_pic=@fileName WHERE SoftToken=@AdminSoftToken and Id=@UserID";

                            var updateRequest = await conn.ExecuteAsync(updateQuery, new
                            {

                                superAdminUpdateProfileReq.username,
                                superAdminUpdateProfileReq.fullname,
                                superAdminUpdateProfileReq.phonenumber,
                                superAdminUpdateProfileReq.Email,
                                fileName,
                                superAdminUpdateProfileReq.AdminSoftToken,
                                superAdminUpdateProfileReq.UserID

                            });

                            if (updateRequest >= 1)
                            {
                                // Log Activities
                                var syslog = new Sys_logReq()
                                {
                                    channel = _appChannel.Channel2,
                                    description = $"Updated profile  Successfully {superAdminUpdateProfileReq.username}",
                                    UserID = superAdminUpdateProfileReq.UserID.ToString(),
                                    Usertype = _appSystemType.Type1
                                };
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Updated profile  Successfully");
                                var sys_log = await _systemUtility.sys_Log(syslog);

                            }
                            else
                            {
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "SoftToken not valid");
                            }
                        }
                    }
                }
                else
                {
                    // validation for password
                    var password = Cipher.GenerateHash(superAdminUpdateProfileReq.password);
                    var cpassword = Cipher.GenerateHash(superAdminUpdateProfileReq.confirmpassword);
                    if (password != cpassword)
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Passwords does not match");
                    }
                    //if (superAdminUpdateProfileReq.profile_pic.Length >= ImageMaximumBytes)
                    //{
                    //    response = new ResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Profile Picture size too much,kindly upload smaller size picture");
                    //}
                    if (Path.GetExtension(superAdminUpdateProfileReq.profile_pic.FileName).ToLower() != ".jpg"
                && Path.GetExtension(superAdminUpdateProfileReq.profile_pic.FileName).ToLower() != ".png"
                && Path.GetExtension(superAdminUpdateProfileReq.profile_pic.FileName).ToLower() != ".jpeg")
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Profile Picture is not a valid image file type");
                    }

                    //admiin picture

                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    if (superAdminUpdateProfileReq.profile_pic.FileName.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(superAdminUpdateProfileReq.profile_pic.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            superAdminUpdateProfileReq.profile_pic.CopyTo(stream);
                        }

                        //return Ok(new { dbPath });

                        //update database
                        using (IDbConnection conn = Connection)
                        {

                            string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_SuperAdmin] SET username = @username,fullname=@fullname,phonenumber=@phonenumber,password=@password,Email=@Email,profile_pic=@fileName WHERE SoftToken=@AdminSoftToken and Id=@UserID";

                            var updateRequest = await conn.ExecuteAsync(updateQuery, new
                            {

                                superAdminUpdateProfileReq.username,
                                superAdminUpdateProfileReq.fullname,
                                superAdminUpdateProfileReq.phonenumber,
                                password,
                                superAdminUpdateProfileReq.Email,
                                fileName,
                                superAdminUpdateProfileReq.AdminSoftToken,
                                superAdminUpdateProfileReq.UserID
                            });

                            if (updateRequest >= 1)
                            {
                                // Log Activities
                                var syslog = new Sys_logReq()
                                {
                                    channel = _appChannel.Channel2,
                                    description = $"Updated profile  Successfully {superAdminUpdateProfileReq.username}",
                                    UserID = superAdminUpdateProfileReq.UserID.ToString(),
                                    Usertype = _appSystemType.Type1
                                };
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Updated profile  Successfully");
                                var sys_log = await _systemUtility.sys_Log(syslog);

                            }
                            else
                            {
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "SoftToken not valid");
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                _logger.LogError($"[PNA][Update Super Admin][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }
    }
}
