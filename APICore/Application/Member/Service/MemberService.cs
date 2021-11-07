using APICore.Application.Member.Interface;
using AppCore.Application.TransactionNotification;
using Dapper;
using Dapper.Contrib.Extensions;
using Domain.Application.Member.DTO;
using Domain.Application.SuperMember.DTO;
using Domain.Application.SystemUtility.Sys_Logs;
using Domain.DTO.Notification.Email;
using Domain.Models;
using Microsoft.AspNetCore.Http;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;
using Utilities.Interface;
using Utilities.SystemActivity.Interface;

namespace APICore.Application.Member.Service
{
    public class MemberService : IMemberService
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

        public MemberService(Logger logger, IOptions<AppKeys> appKeys, IConfiguration configuration, ISystemActivityService systemUtility, IOptions<AppSystemType> appsystemtype, IOptions<AppChannel> channel, ISharedService sharedService, INotificationServices notificationServices, IOptions<BaseUrls> baseUrls, UIHttpClient httpClient, ITransactionNotification transactionNotification)
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

        public async Task<ResponseParam> Changepassword(ChangePasswordReq cooperatorChangePasswordReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);

            try
            {
                var CheckPassword = ValidatePassword(cooperatorChangePasswordReq.New_Password, out string ErroMessage);
                if (string.IsNullOrEmpty(cooperatorChangePasswordReq.New_Password))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "New_Password is required");
                }
                if (cooperatorChangePasswordReq.New_Password != cooperatorChangePasswordReq.Confirm_Password)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Password Didn't Matched kindly retry");
                }
                else if (CheckPassword == false)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, $"{ErroMessage}");
                }
                else
                {
                    IEnumerable<UserProfiledLoginDetails> userresults;
                    //Check for user
                    using (IDbConnection conn = Connection)
                    {
                        string selectQuery = @"SELECT * FROM [DB_A57DC4_pnaDb_MemberAuth] where Email=@Email";
                        userresults = await conn.QueryAsync<UserProfiledLoginDetails>(selectQuery, new
                        {
                            cooperatorChangePasswordReq.Email
                        });
                        int row = userresults.Count();
                        if (row >= 1)
                        {
                            foreach (var userauthinfo in userresults)
                            {
                                var Password = Cipher.GenerateHash(cooperatorChangePasswordReq.Current_Password);
                                using (IDbConnection con = Connection)
                                {
                                    string selectQueryauth = @"SELECT * FROM [DB_A57DC4_pnaDb_MemberAuth] where Password=@Password";

                                    var Superadminresults = await con.QueryAsync(selectQueryauth, new
                                    {
                                        Password
                                    });
                                    int row2 = Superadminresults.Count();
                                    if (row2 >= 1)
                                    {
                                        using (IDbConnection connu = Connection)
                                        {
                                            var newPassword = Cipher.GenerateHash(cooperatorChangePasswordReq.New_Password);
                                            string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_MemberAuth] SET Password = @newPassword WHERE PhoneNumber = @PhoneNumber";

                                            var updateRequest = await connu.ExecuteAsync(updateQuery, new
                                            {
                                                newPassword,
                                                userauthinfo.PhoneNumber
                                            });

                                            if (updateRequest >= 1)
                                            {
                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Updated Password Successfully");
                                                var syslog = new Sys_logReq()
                                                {
                                                    channel = _appChannel.Channel1,
                                                    description = $"{userauthinfo.Email} Updated Password Details Successfully",
                                                    UserID = userauthinfo.Email,
                                                    Usertype = _appSystemType.Type8
                                                };
                                                var sys_log = await _systemUtility.sys_Log(syslog);
                                            }
                                            else
                                            {
                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "System Down retry in a moment");
                                                var syslog = new Sys_logReq()
                                                {
                                                    channel = _appChannel.Channel1,
                                                    description = $"{userauthinfo.Email} Unable to Update Password Details",
                                                    UserID = userauthinfo.Email,
                                                    Usertype = _appSystemType.Type8
                                                };
                                                var sys_log = await _systemUtility.sys_Log(syslog);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid Current Password");
                                    }
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "No such User");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[PNAAPI][user change password][Response] => {ex.Message} | [Loginrequest]=> " +
                                                 JsonConvert.SerializeObject(cooperatorChangePasswordReq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> ChangeTransactionpin(ChangeTransactionPinReq changeTransactionPinReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrWhiteSpace(changeTransactionPinReq.WalletId))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId is required");
                }
                else if (!changeTransactionPinReq.WalletId.All(c => char.IsNumber(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId must be a number");
                }
                else if (!changeTransactionPinReq.Old_TransactionPin.All(c => char.IsNumber(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Old_TransactionPin must be a number");
                }
                else if (!changeTransactionPinReq.New_TransactionPin.All(c => char.IsNumber(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "New_TransactionPin must be a number");
                }
                else
                {
                    //check for Cooperator

                    IEnumerable<Userdetails> userauthresults;
                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@WalletId";

                        userauthresults = await connlogon.QueryAsync<Userdetails>(selectQuerylogon, new
                        {
                            changeTransactionPinReq.WalletId
                        });
                        int rowlogon = userauthresults.Count();
                        // log into database
                        if (rowlogon >= 1)
                        {
                            //check if user exist
                            var checkTransactionPin = Cipher.GenerateHash(changeTransactionPinReq.Old_TransactionPin);

                            foreach (var authinfo in userauthresults)
                            {
                                if (authinfo.IsActivated == false)
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Kindly Setup your mobile app");
                                }
                                else if (authinfo.TransactionPinSetupSatus == false)
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Transaction Pin is not set, kindly set it up first.");
                                }
                                else if (authinfo.TransactionPin != checkTransactionPin)
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid Current Transaction Pin, kindly Retry");
                                }
                                else
                                {
                                    var TransactionPin = Cipher.GenerateHash(changeTransactionPinReq.New_TransactionPin.ToString());

                                    string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_Member] SET TransactionPin = @TransactionPin WHERE WalletId = @WalletId";

                                    var updateRequest = await connlogon.ExecuteAsync(updateQuery, new
                                    {
                                        TransactionPin,
                                        changeTransactionPinReq.WalletId
                                    });
                                    if (updateRequest >= 1)
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Transaction Pin Changed successfully");
                                        var syslog = new Sys_logReq()
                                        {
                                            channel = _appChannel.Channel1,
                                            description = $"{authinfo.Email} change Transaction Pin Successfully",
                                            UserID = authinfo.Email,
                                            Usertype = _appSystemType.Type8
                                        };
                                        var sys_log = await _systemUtility.sys_Log(syslog);
                                    }
                                    else
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Failed Try again in a moment");
                                        var syslog = new Sys_logReq()
                                        {
                                            channel = _appChannel.Channel1,
                                            description = $"{authinfo.Email} Unable to change Transaction Pin",
                                            UserID = authinfo.Email,
                                            Usertype = _appSystemType.Type8
                                        };
                                        var sys_log = await _systemUtility.sys_Log(syslog);
                                    }
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "OOps Nothing found for you");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][change transaction pin][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][change transaction pin][Response] => {ex.Message} | [request]=> " +
                                                  $"{changeTransactionPinReq} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> debit_PNA(Debit_PNAreq debit_PNAreq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                var Initiate_debitDTO = new Initiate_debitDTO()
                {
                    amount = debit_PNAreq.amount,
                    currency = "NGN",
                    destinationAccountNumber = debit_PNAreq.destinationAccountNumber,
                    destinationBankCode = debit_PNAreq.destinationBankCode,
                    narration = debit_PNAreq.narration,
                    reference = debit_PNAreq.reference,
                    sourceAccountNumber = _appKeys.MonnifyWalletAccount
                };
                string endpoint = $"{_baseUrls.monnifyUrl2}disbursements/single";
                var reqBody = JsonConvert.SerializeObject(Initiate_debitDTO);
                var init_transactionResponse = await _httpClient.Post(reqBody, endpoint, requestId);
                var init_transactionDTO = JsonConvert.DeserializeObject<debit_PNADTO>(init_transactionResponse);
                if (init_transactionDTO.responseCode == "0")
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "ok", init_transactionDTO);
                }
                else
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Error", init_transactionDTO);
                }
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[PNAAPI][Monnify Debit PNA][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> Debit_Wallet(Debit_WalletReq debit_WalletRe)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);

            string TransactionReference = Guid.NewGuid().ToString("N").ToUpper().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string Generatepassword = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string Generateusername = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            var logmonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
            //string WalletId = Helper.GenerateUniqueId(6);
            //double ReferralLimit = 1000;
            try
            {
                if (!debit_WalletRe.WalletId.All(c => char.IsDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId must be a number");
                }
                else if (!debit_WalletRe.TransactionPin.All(c => char.IsDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "TransactionPin must be a number");
                }
                else
                {
                    IEnumerable<PnaMember> senderresults;
                    IEnumerable<WalletDTO> Senderwalletresults;

                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@WalletId";

                        senderresults = await connlogon.QueryAsync<PnaMember>(selectQuerylogon, new
                        {
                            debit_WalletRe.WalletId
                        });
                        int rowlogon = senderresults.Count();
                        if (rowlogon >= 1)
                        {
                            foreach (var authinfo in senderresults)
                            {
                                //step 2 -- check Sender wallet balance
                                string selectQuerylogon3 = @"SELECT * FROM [DB_A57DC4_pnaDb_Wallet] where WalletId=@WalletId";

                                Senderwalletresults = await connlogon.QueryAsync<WalletDTO>(selectQuerylogon3, new
                                {
                                    debit_WalletRe.WalletId
                                });
                                int rowlogon3 = Senderwalletresults.Count();

                                if (rowlogon3 >= 1)
                                {
                                    foreach (var WalletInfo in Senderwalletresults)
                                    {
                                        //step 3-- validate transfer amount against wallet balance

                                        if (debit_WalletRe.amount > WalletInfo.WalletBalance)
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INSUFFICIENT_BAL, "INSUFFICIENT BALANCE");
                                        }
                                        else
                                        {
                                            //step 4-- - check / validate sender transaction pin
                                            var transactionpin = Cipher.GenerateHash(debit_WalletRe.TransactionPin.ToString());
                                            if (transactionpin != authinfo.TransactionPin)
                                            {
                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid Transaction Pin, Kindly Retry");
                                            }
                                            else
                                            {
                                                //step 5--- debit  amount from sender wallet
                                                double debit = WalletInfo.WalletBalance - debit_WalletRe.amount;
                                                //step 6-- - update sender wallet balance
                                                string updateQuerybc = @"UPDATE [DB_A57DC4_pnaDb_Wallet] SET WalletBalance = @debit WHERE WalletId=@WalletId";

                                                var updatesenderwalletbalance = await connlogon.ExecuteAsync(updateQuerybc, new
                                                {
                                                    debit,

                                                    debit_WalletRe.WalletId
                                                });
                                                if (updatesenderwalletbalance >= 1)
                                                {
                                                    var debit_WalletDTO = new debit_WalletDTO()
                                                    {
                                                        IsTransaction_status = "SUCCESS",
                                                        Wallet_balance = debit,
                                                        WalletId = authinfo.WalletId
                                                    };

                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Ok", debit_WalletDTO);
                                                }
                                                else
                                                {
                                                    var debit_WalletDTO = new debit_WalletDTO()
                                                    {
                                                        IsTransaction_status = "Failed",
                                                        WalletId = authinfo.WalletId
                                                    };
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Could not process Debit_Wallet Request");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid Member WalletId");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][Debit_Wallet][Response] => {ex.Message} | [requestId]=> {requestId}");

                await _systemUtility.JsonErrorLog($"[PNAAPI][Debit_Wallet][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> FundWalletExtention(fundWalletReq fundWalletReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);

            string TransactionReference = Guid.NewGuid().ToString("N").ToUpper().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string Generatepassword = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string paymentReference = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 15);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            var logmonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
            //string WalletId = Helper.GenerateUniqueId(6);
            //double ReferralLimit = 1000;

            try
            {
                if (string.IsNullOrWhiteSpace(fundWalletReq.WalletId))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId is required");
                }
                else
                {
                    //step 1-- check/validate sender info with walletid

                    IEnumerable<Userdetails> senderresults;

                    IEnumerable<WalletDTO> Senderwalletresults;

                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@WalletId";

                        senderresults = await connlogon.QueryAsync<Userdetails>(selectQuerylogon, new
                        {
                            fundWalletReq.WalletId
                        });
                        int rowlogon = senderresults.Count();

                        if (rowlogon >= 1)
                        {
                            foreach (var authinfo in senderresults)
                            {
                                //step 2 -- check Sender wallet balance
                                string selectQuerylogon3 = @"SELECT * FROM [DB_A57DC4_pnaDb_Wallet] where WalletId=@WalletId";

                                Senderwalletresults = await connlogon.QueryAsync<WalletDTO>(selectQuerylogon3, new
                                {
                                    fundWalletReq.WalletId
                                });
                                int rowlogon3 = Senderwalletresults.Count();

                                if (rowlogon3 >= 1)
                                {
                                    foreach (var WalletInfo in Senderwalletresults)
                                    {
                                        // initiate monnify
                                        string paymentDescription = "PNA Wallet funding";
                                        string json = File.ReadAllText(@"Resources\init_transaction.json");
                                        dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                                        jsonObj["amount"] = $"{fundWalletReq.Amount}";
                                        jsonObj["customerName"] = $"{authinfo.FullName}";
                                        jsonObj["customerEmail"] = $"{authinfo.Email}";
                                        jsonObj["paymentReference"] = $"{paymentReference}";
                                        jsonObj["paymentDescription"] = $"{paymentDescription}";
                                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                                        File.WriteAllText(@"Resources\init_transaction.json", output);

                                        string endpoint = $"{_baseUrls.monnifyUrl}merchant/transactions/init-transaction";
                                        var reqBody = JsonConvert.SerializeObject(jsonObj);
                                        var init_transactionResponse = await _httpClient.Post(reqBody, endpoint, requestId);
                                        var init_transactionDTO = JsonConvert.DeserializeObject<init_transactionDTO>(init_transactionResponse);

                                        if (init_transactionDTO.responseCode == "0")
                                        {
                                            var FundwalletDTO = new FundwalletDTO()
                                            {
                                                checkoutUrl = init_transactionDTO.responseBody.checkoutUrl,
                                                paymentReference = init_transactionDTO.responseBody.paymentReference
                                            };

                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, $"{init_transactionDTO.responseMessage}", FundwalletDTO);

                                            // insert payment instance to the db
                                            var FundWalletHistory = new FundWalletHistory()
                                            {
                                                paymentReference = FundwalletDTO.paymentReference,
                                                amount = fundWalletReq.Amount,
                                                IsWalletFunded = false,
                                                WalletId = fundWalletReq.WalletId
                                            };
                                            using (IDbConnection conn = Connection)
                                            {
                                                conn.Insert(FundWalletHistory);
                                            }
                                        }
                                        else
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"{init_transactionDTO.responseMessage}");
                                        }
                                    }
                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId does not have wallet balance");
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid WalletId");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[PNAAPI][Fundwallet][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> GetProfileByLoginToken(string Logintoken)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);

            try
            {
                if (string.IsNullOrEmpty(Logintoken))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "LoginToken is required");
                }

                IEnumerable<MemberauthDTO> userresults;
                IEnumerable<MemberAuthResponse> userauthresults;
                using (IDbConnection connlogon = Connection)
                {
                    string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_MemberAuth] where Token=@Logintoken";

                    userauthresults = await connlogon.QueryAsync<MemberAuthResponse>(selectQuerylogon, new
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
                                    string selectQuery = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where PhoneNumbers=@PhoneNumber";

                                    userresults = await conn.QueryAsync<MemberauthDTO>(selectQuery, new
                                    {
                                        authinfo.PhoneNumber
                                    });

                                    int row = userresults.Count();
                                    if (row >= 1)
                                    {
                                        foreach (var userauthinfo in userresults)
                                        {
                                            if (userauthinfo.IsActivated == false)
                                            {
                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "You need to activate your app");
                                            }
                                            else
                                            {
                                                string sql = "SELECT * FROM DB_A57DC4_pnaDb_Member WHERE WalletId = @WalletId; SELECT * FROM DB_A57DC4_pnaDb_MemberType WHERE Id = @MemberTypeId;SELECT * FROM DB_A57DC4_pnaDb_Wallet WHERE WalletId = @WalletId; ";

                                                using (var connection = Connection)
                                                {
                                                    connection.Open();

                                                    using (var multi = connection.QueryMultiple(sql, new { WalletId = userauthinfo.WalletId, userauthinfo.MemberTypeId }))
                                                    {
                                                        var Member = multi.ReadSingle<MemberDetails>();
                                                        var MembershipType = multi.Read<Membertype>().ToList();
                                                        var GetWalletBalance = multi.Read<GetWalletBalanceDTO>().ToList();
                                                        Member.MembershipType = new List<Membertype>();
                                                        Member.WalletBalance = new List<GetWalletBalanceDTO>();
                                                        Member.MembershipType.AddRange(MembershipType);
                                                        Member.WalletBalance.AddRange(GetWalletBalance);
                                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "User Info", Member);
                                                    }
                                                    //
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "No such Member");
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
                _logger.LogError($"[MM][LoginToken][Response] => {ex.Message} | [Loginrequest]=> " +
                                                  $"{Logintoken}  | [requestId]=> {requestId}");

                await _systemUtility.JsonErrorLog($"[PNAAPI][Create new member][Response] => {ex.Message} | [request]=> " +
                                                 JsonConvert.SerializeObject(Logintoken) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> GetWalletBalance(string WalletId)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrEmpty(WalletId))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId is required");
                }
                else if (!WalletId.All(c => char.IsNumber(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId must be a number");
                }
                else
                {
                    // Query the database for a requested

                    using (IDbConnection conn = Connection)
                    {
                        //string selectQuery = @"SELECT COUNT(*) FROM [DB_A57DC4_mainmartDb_Agent]";

                        //var Sys_Permissionsresults = await conn.QueryAsync(selectQuery, new
                        //{
                        //});
                        double commissionamount = conn.ExecuteScalar<double>($"SELECT WalletBalance FROM DB_A57DC4_pnaDb_Wallet where WalletId='{WalletId}'");
                        // int rowag = Sys_Permissionsresults.Count();
                        var GetWalletBalanceDTO = new GetWalletBalanceDTO()
                        {
                            WalletBalance = commissionamount
                        };
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Success", GetWalletBalanceDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][Get wallet balance][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][Get wallet balance][Response] => {ex.Message} | [request]=> " +
                                                 JsonConvert.SerializeObject(WalletId) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> InitializeFundWallet(InitializeReq initializeReq)
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
                IEnumerable<InitializeFundWalletDTO> senderresults;
                IEnumerable<WalletDTO> Senderwalletresults;
                IEnumerable<Userdetails> userresults;
                using (IDbConnection connlogon = Connection)
                {
                    string selectQuerylogon = @"SELECT * FROM [FundWalletHistory] where paymentReference=@paymentReference";

                    senderresults = await connlogon.QueryAsync<InitializeFundWalletDTO>(selectQuerylogon, new
                    {
                        initializeReq.paymentReference
                    });
                    int rowlogon = senderresults.Count();

                    if (rowlogon >= 1)
                    {
                        foreach (var authinfo in senderresults)
                        {
                            //call the query transaction endpoint
                            string endpoint = $"{_baseUrls.monnifyUrl}merchant/transactions/query?paymentReference={initializeReq.paymentReference}";
                            var Getqueryresponse = await _httpClient.Get(endpoint, requestId);
                            var GetqueryDTOs = JsonConvert.DeserializeObject<GetqueryDTOs>(Getqueryresponse);
                            if (GetqueryDTOs.responseBody.paymentStatus == "PENDING")
                            {
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"Payment for this paymentReference: {initializeReq.paymentReference} has not been Initialize kindly make payment");
                            }
                            else if (GetqueryDTOs.responseBody.paymentStatus == "PAID")
                            {
                                using (IDbConnection conn = Connection)
                                {
                                    var id = authinfo.Id;
                                    var fundWalletHistory = conn.Get<FundWalletHistory>(id);

                                    fundWalletHistory.amount = GetqueryDTOs.responseBody.amount;
                                    fundWalletHistory.amountPaid = GetqueryDTOs.responseBody.amountPaid;
                                    fundWalletHistory.completed = GetqueryDTOs.responseBody.completed;
                                    fundWalletHistory.completedOn = GetqueryDTOs.responseBody.completedOn;
                                    fundWalletHistory.createdOn = GetqueryDTOs.responseBody.createdOn;
                                    fundWalletHistory.currencyCode = GetqueryDTOs.responseBody.currencyCode;
                                    fundWalletHistory.customerEmail = GetqueryDTOs.responseBody.customerEmail;
                                    fundWalletHistory.customerName = GetqueryDTOs.responseBody.customerName;
                                    fundWalletHistory.IsWalletFunded = true;
                                    fundWalletHistory.payableAmount = Convert.ToInt32(GetqueryDTOs.responseBody.payableAmount);
                                    fundWalletHistory.paymentDescription = GetqueryDTOs.responseBody.paymentDescription;
                                    fundWalletHistory.paymentMethod = GetqueryDTOs.responseBody.paymentMethod;
                                    fundWalletHistory.paymentStatus = GetqueryDTOs.responseBody.paymentStatus;
                                    fundWalletHistory.transactionReference = GetqueryDTOs.responseBody.transactionReference;
                                    var checkquery = conn.Update(fundWalletHistory);
                                    if (checkquery == true)
                                    {
                                        //Get User details
                                        string selectQuerylogon2 = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@WalletId";

                                        userresults = await connlogon.QueryAsync<Userdetails>(selectQuerylogon2, new
                                        {
                                            fundWalletHistory.WalletId
                                        });

                                        foreach (var userinfo in userresults)
                                        {
                                            //-- check User wallet balance
                                            string selectQuerylogon3 = @"SELECT * FROM [DB_A57DC4_pnaDb_Wallet] where WalletId=@WalletId";

                                            Senderwalletresults = await connlogon.QueryAsync<WalletDTO>(selectQuerylogon3, new
                                            {
                                                fundWalletHistory.WalletId
                                            });

                                            foreach (var WalletInfo in Senderwalletresults)
                                            {
                                                //credit  amount to  wallet
                                                double credit = WalletInfo.WalletBalance + fundWalletHistory.amount;
                                                //step 9-- update  wallet balance
                                                string updateQuerybcw = @"UPDATE [DB_A57DC4_pnaDb_Wallet] SET WalletBalance = @credit WHERE WalletId=@WalletId";

                                                var updaterecieverwalletbalance = await connlogon.ExecuteAsync(updateQuerybcw, new
                                                {
                                                    credit,

                                                    fundWalletHistory.WalletId
                                                });
                                                if (updaterecieverwalletbalance >= 1)
                                                {
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Wallet Funding successful");
                                                    // send sms
                                                    // step 11-- send sms to both(Dr, Cr)

                                                    string Crmsg = $"Your Wallet Account Has Been Funded With  N{fundWalletHistory.amount.ToString("N", CultureInfo.CurrentCulture)} on {logdate.ToString("G", DateTimeFormatInfo.InvariantInfo)} By PWF:PNA App/Fund-Wallet";

                                                    //var Crsms = await _notificationServices.SendSMS(authinfo.PhoneNumber, Crmsg);
                                                    string json = File.ReadAllText(@"Resources\emp.json");
                                                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                                                    jsonObj["SMS"]["message"]["messagetext"] = $"{Crmsg}";
                                                    jsonObj["SMS"]["recipients"]["gsm"][0]["msidn"] = $"{userinfo.PhoneNumbers}";
                                                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                                                    File.WriteAllText(@"Resources\emp.json", output);

                                                    string endpoint2 = $"{_baseUrls.SMSAPI}";
                                                    var reqBody = JsonConvert.SerializeObject(jsonObj);
                                                    var SmsResponse = await _httpClient.Post(reqBody, endpoint2, requestId);
                                                }
                                                else
                                                {
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Could not process Fund wallet Request. Please retry in a moment");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Could not process fundWalletHistory Request. Please retry in a moment");
                                    }
                                }
                            }
                            else if (GetqueryDTOs.responseBody.paymentStatus == "FAILED")
                            {
                                using (IDbConnection connn = Connection)
                                {
                                    var idd = authinfo.Id;
                                    var fundWalletHistoryd = connn.Get<FundWalletHistory>(idd);

                                    fundWalletHistoryd.amount = GetqueryDTOs.responseBody.amount;
                                    fundWalletHistoryd.amountPaid = GetqueryDTOs.responseBody.amountPaid;
                                    fundWalletHistoryd.completed = GetqueryDTOs.responseBody.completed;
                                    fundWalletHistoryd.completedOn = GetqueryDTOs.responseBody.completedOn;
                                    fundWalletHistoryd.createdOn = GetqueryDTOs.responseBody.createdOn;
                                    fundWalletHistoryd.currencyCode = GetqueryDTOs.responseBody.currencyCode;
                                    fundWalletHistoryd.customerEmail = GetqueryDTOs.responseBody.customerEmail;
                                    fundWalletHistoryd.customerName = GetqueryDTOs.responseBody.customerName;
                                    fundWalletHistoryd.IsWalletFunded = false;
                                    fundWalletHistoryd.payableAmount = Convert.ToInt32(GetqueryDTOs.responseBody.payableAmount);
                                    fundWalletHistoryd.paymentDescription = GetqueryDTOs.responseBody.paymentDescription;
                                    fundWalletHistoryd.paymentMethod = GetqueryDTOs.responseBody.paymentMethod;
                                    fundWalletHistoryd.paymentStatus = GetqueryDTOs.responseBody.paymentStatus;
                                    fundWalletHistoryd.transactionReference = GetqueryDTOs.responseBody.transactionReference;
                                    var checkqueryd = connn.Update(fundWalletHistoryd);
                                    if (checkqueryd == true)
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"Payment {GetqueryDTOs.responseBody.paymentStatus}. Please retry and fund wallet again");
                                    }
                                }
                            }
                            else
                            {
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"Payment {GetqueryDTOs.responseBody.paymentStatus}");
                            }
                        }
                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid paymentReference");
                    }
                }
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[PNAAPI][InitializeFundWallet][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> InitializeMembershipSubcription(InitializeReq initializeReq)
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
                IEnumerable<InitializeMembershipSubscriptionDTO> senderresults;
                //<WalletDTO> Senderwalletresults;
                IEnumerable<Userdetails> userresults;
                IEnumerable<PnaMemberType> MemberTyperesults;
                using (IDbConnection connlogon = Connection)
                {
                    string selectQuerylogon = @"SELECT * FROM [MembershipSubscription] where paymentReference=@paymentReference";

                    senderresults = await connlogon.QueryAsync<InitializeMembershipSubscriptionDTO>(selectQuerylogon, new
                    {
                        initializeReq.paymentReference
                    });

                    int rowlogon = senderresults.Count();

                    if (rowlogon >= 1)
                    {
                        foreach (var authinfo in senderresults)
                        {
                            //Get User details
                            string selectQuerylogon3 = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@WalletId";

                            userresults = await connlogon.QueryAsync<Userdetails>(selectQuerylogon3, new
                            {
                                authinfo.WalletId
                            });

                            foreach (var userinfo in userresults)
                            {
                                string selectQuerylogon4 = @"SELECT * FROM [DB_A57DC4_pnaDb_MemberType] where Id=@MemberTypeId";

                                MemberTyperesults = await connlogon.QueryAsync<PnaMemberType>(selectQuerylogon4, new
                                {
                                    userinfo.MemberTypeId
                                });
                                foreach (var MemberTypeInfo in MemberTyperesults)
                                {
                                    if (userinfo.IsActivated == false)
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Your account is not active");
                                    }
                                    else if (userinfo.subscriptionsStatus == "subscribed")
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "You have an active Membership plan");
                                    }
                                    else
                                    {
                                        //call the query transaction endpoint
                                        string endpoint = $"{_baseUrls.monnifyUrl}merchant/transactions/query?paymentReference={initializeReq.paymentReference}";
                                        var Getqueryresponse = await _httpClient.Get(endpoint, requestId);
                                        var GetqueryDTOs = JsonConvert.DeserializeObject<GetqueryDTOs>(Getqueryresponse);
                                        if (GetqueryDTOs.responseBody.paymentStatus == "PENDING")
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"Payment for this paymentReference: {initializeReq.paymentReference} has not been Initialize kindly make payment");
                                        }
                                        else if (GetqueryDTOs.responseBody.paymentStatus == "PAID")
                                        {
                                            using (IDbConnection conn = Connection)
                                            {
                                                var id = authinfo.Id;
                                                var fundWalletHistory = conn.Get<MembershipSubscription>(id);

                                                fundWalletHistory.amount = GetqueryDTOs.responseBody.amount;
                                                fundWalletHistory.amountPaid = GetqueryDTOs.responseBody.amountPaid;
                                                fundWalletHistory.completed = GetqueryDTOs.responseBody.completed;
                                                fundWalletHistory.completedOn = GetqueryDTOs.responseBody.completedOn;
                                                fundWalletHistory.createdOn = GetqueryDTOs.responseBody.createdOn;
                                                fundWalletHistory.currencyCode = GetqueryDTOs.responseBody.currencyCode;
                                                fundWalletHistory.customerEmail = GetqueryDTOs.responseBody.customerEmail;
                                                fundWalletHistory.customerName = GetqueryDTOs.responseBody.customerName;
                                                fundWalletHistory.IsMembershipsubscribed = true;
                                                fundWalletHistory.MemberType = MemberTypeInfo.Type;
                                                fundWalletHistory.membership_duedate = DateTime.Today.AddYears(1);
                                                fundWalletHistory.payableAmount = Convert.ToInt32(GetqueryDTOs.responseBody.payableAmount);
                                                fundWalletHistory.paymentDescription = GetqueryDTOs.responseBody.paymentDescription;
                                                fundWalletHistory.paymentMethod = GetqueryDTOs.responseBody.paymentMethod;
                                                fundWalletHistory.paymentStatus = GetqueryDTOs.responseBody.paymentStatus;
                                                fundWalletHistory.transactionReference = GetqueryDTOs.responseBody.transactionReference;
                                                var checkquery = conn.Update(fundWalletHistory);
                                                if (checkquery == true)
                                                {
                                                    //Get User details
                                                    string selectQuerylogon2 = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@WalletId";

                                                    userresults = await connlogon.QueryAsync<Userdetails>(selectQuerylogon2, new
                                                    {
                                                        fundWalletHistory.WalletId
                                                    });

                                                    foreach (var userinfo2 in userresults)
                                                    {
                                                        //update member
                                                        string MembershipID = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 15);
                                                        string subscriptionsStatus = "subscribed";
                                                        string getMembershipID = "";
                                                        if (userinfo2.MembershipID == null)
                                                        {
                                                            getMembershipID = MembershipID;
                                                        }
                                                        else
                                                        {
                                                            getMembershipID = userinfo2.MembershipID;
                                                        }

                                                        string updateQuerybcw = @"UPDATE [DB_A57DC4_pnaDb_Member] SET subscriptionsStatus = @subscriptionsStatus,MembershipID=@getMembershipID WHERE WalletId=@WalletId";

                                                        var updaterecieverwalletbalance = await connlogon.ExecuteAsync(updateQuerybcw, new
                                                        {
                                                            subscriptionsStatus,
                                                            getMembershipID,
                                                            fundWalletHistory.WalletId
                                                        });
                                                        string updateQuerybcw33 = @"UPDATE [DB_A57DC4_pnaDb_MemberAuth] SET MembershipID=@getMembershipID WHERE Email=@Email";

                                                        var updaterecieverwalletbalance3 = await connlogon.ExecuteAsync(updateQuerybcw33, new
                                                        {
                                                            getMembershipID,
                                                            userinfo2.Email
                                                        });
                                                        if (updaterecieverwalletbalance >= 1 && updaterecieverwalletbalance3 >= 1)
                                                        {
                                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Membership subscription payment successful");
                                                        }
                                                        else
                                                        {
                                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Could not process Membership subscription payment Request. Please retry in a moment");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Could not process Membership subscription payment Request. Please retry in a moment");
                                                }
                                            }
                                        }
                                        else if (GetqueryDTOs.responseBody.paymentStatus == "FAILED")
                                        {
                                            using (IDbConnection connn = Connection)
                                            {
                                                var idd = authinfo.Id;
                                                var fundWalletHistoryd = connn.Get<MembershipSubscription>(idd);

                                                fundWalletHistoryd.amount = GetqueryDTOs.responseBody.amount;
                                                fundWalletHistoryd.amountPaid = GetqueryDTOs.responseBody.amountPaid;
                                                fundWalletHistoryd.completed = GetqueryDTOs.responseBody.completed;
                                                fundWalletHistoryd.completedOn = GetqueryDTOs.responseBody.completedOn;
                                                fundWalletHistoryd.createdOn = GetqueryDTOs.responseBody.createdOn;
                                                fundWalletHistoryd.currencyCode = GetqueryDTOs.responseBody.currencyCode;
                                                fundWalletHistoryd.customerEmail = GetqueryDTOs.responseBody.customerEmail;
                                                fundWalletHistoryd.customerName = GetqueryDTOs.responseBody.customerName;
                                                fundWalletHistoryd.IsMembershipsubscribed = false;
                                                fundWalletHistoryd.MemberType = MemberTypeInfo.Type;
                                                fundWalletHistoryd.payableAmount = Convert.ToInt32(GetqueryDTOs.responseBody.payableAmount);
                                                fundWalletHistoryd.paymentDescription = GetqueryDTOs.responseBody.paymentDescription;
                                                fundWalletHistoryd.paymentMethod = GetqueryDTOs.responseBody.paymentMethod;
                                                fundWalletHistoryd.paymentStatus = GetqueryDTOs.responseBody.paymentStatus;
                                                fundWalletHistoryd.transactionReference = GetqueryDTOs.responseBody.transactionReference;
                                                var checkqueryd = connn.Update(fundWalletHistoryd);
                                                if (checkqueryd == true)
                                                {
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"Payment {GetqueryDTOs.responseBody.paymentStatus}. Please retry and fund wallet again");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"Payment {GetqueryDTOs.responseBody.paymentStatus}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid paymentReference");
                    }
                }
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[PNAAPI][Initialize MembershipSubscription][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> MemberLogin(MLoginReq loginReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrEmpty(loginReq.password))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Password field is required");
                }
                else if (string.IsNullOrEmpty(loginReq.phonemail))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Phone  or Email field is required");
                }

                // encrypt password field
                var Password = Cipher.GenerateHash(loginReq.password);
                var PhoneNumber = loginReq.phonemail;
                // check the database table
                using (IDbConnection connlogon = Connection)
                {
                    string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_MemberAuth] where (PhoneNumber=@PhoneNumber or Email=@PhoneNumber)  and Password=@Password";

                    var loginRequestresults = await connlogon.QueryAsync(selectQuerylogon, new
                    {
                        PhoneNumber,
                        Password
                    });
                    int rowlogon = loginRequestresults.Count();
                    // log into database

                    if (rowlogon >= 1)
                    {
                        // Generate Token
                        //string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        //byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                        //byte[] key = Guid.NewGuid().ToByteArray();
                        //string token = Convert.ToBase64String(time.Concat(key).ToArray());
                        string token = Guid.NewGuid().ToString("N");

                        string GetSessionID = Guid.NewGuid().ToString("N").ToUpper().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 5);
                        string SessionID = Cipher.GenerateHash(GetSessionID);
                        //update Auth Table
                        var TokenExpiry = DateTime.Now.AddMinutes(5);

                        using (IDbConnection conn = Connection)
                        {
                            string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_MemberAuth] SET Token = @token,TokenExpiry=@TokenExpiry WHERE PhoneNumber = @PhoneNumber or Email=@PhoneNumber";

                            var updateRequest = await conn.ExecuteAsync(updateQuery, new
                            {
                                token,
                                TokenExpiry,
                                PhoneNumber
                            });

                            if (updateRequest >= 1)
                            {
                                // var getUserProfile = await GetSuperadminProfileByLoginToken(token);
                                var getUserProfile = await GetProfileByLoginToken(token);

                                if (getUserProfile.Code == "00")
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Member Info", getUserProfile.Data);
                                    var syslog = new Sys_logReq()
                                    {
                                        channel = _appChannel.Channel1,
                                        description = $"Login Successful {loginReq.phonemail}",
                                        UserID = loginReq.phonemail,
                                        Usertype = _appSystemType.Type8
                                    };
                                    var sys_log = await _systemUtility.sys_Log(syslog);
                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, $"{getUserProfile.Code}", $"{getUserProfile.Message}");
                                }
                            }
                            else
                            {
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Unable to Generate Token");
                            }
                        }
                    }
                    else
                    {
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid Password or Phonenumber/Email, Kindly Retry");

                        var syslog = new Sys_logReq()
                        {
                            channel = _appChannel.Channel1,
                            description = $"Login Failed {loginReq.phonemail}",
                            UserID = loginReq.phonemail,
                            Usertype = _appSystemType.Type8
                        };
                        var sys_log = await _systemUtility.sys_Log(syslog);
                    }
                }
            }
            catch (Exception ex)
            {
                DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
                DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
                var logdate = localTime;
                _logger.LogError($"[MM][Login][Response] => {ex.Message} | [Loginrequest]=> " +
                                    $"{loginReq}  | [requestId]=> {requestId}");

                long unixTime = ((DateTimeOffset)logdate).ToUnixTimeSeconds();
                string JsonResult = JsonConvert.SerializeObject($"[MM][Login][Response] => {ex.Message} | [Loginrequest]=> " +
                                    $"{loginReq}  | [requestId]=> {requestId}");
                string path1 = @"Resources\JsonLog\" + logdate.Year + unixTime + ".json";
                using (var tw = new StreamWriter(path1, true))
                {
                    tw.WriteLine(JsonResult.ToString());
                    tw.Close();
                }

                return new UResponseHandler().HandleException(requestId);
            }
            return response;
        }

        public async Task<ResponseParam> MemberReg(RegReq regReq)
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
            Random Rnd = new Random();
            string ActivationCode = (Rnd.Next(100000, 999999)).ToString();
            string checkphonenumber = regReq.PhoneNumber;
            string[] checkphionenumberarray = { "0803", "081", "0703", "0706", "0706", "0806", "0810", "0813", "0814", "0816", "0903", "0906", "0705", "0805", "0807", "0811", "0815", "0905", "0701", "0802", "0708", "0808", "0812", "0907", "0902", "0809", "0901", "0817", "0818", "0909" };

            try
            {
                //validation
                Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
                bool isValidEmail = regex.IsMatch(regReq.Email);
                if (string.IsNullOrWhiteSpace(regReq.Alias) || string.IsNullOrWhiteSpace(regReq.CountryofResidence) || string.IsNullOrWhiteSpace(regReq.DateofBirth) || string.IsNullOrWhiteSpace(regReq.Email) || string.IsNullOrWhiteSpace(regReq.FullName) || string.IsNullOrWhiteSpace(regReq.Gender) || string.IsNullOrWhiteSpace(regReq.MemberTypeId) || string.IsNullOrWhiteSpace(regReq.PhoneNumber) || string.IsNullOrWhiteSpace(regReq.Profession) || string.IsNullOrWhiteSpace(regReq.referralCode) || string.IsNullOrWhiteSpace(regReq.TellUsAboutYourself) || string.IsNullOrWhiteSpace(regReq.WhyDoYouWantToJoin))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "All Fields are required");
                }
                else if (regReq.PhoneNumber.Length > 11)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "PhoneNumber must not be more than 11 digits");
                }
                else if (regReq.PhoneNumber.Length < 11)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "PhoneNumber must not be less than 11 digits");
                }
                else if (!isValidEmail)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "The email is invalid");
                }
                else if (!checkphionenumberarray.Any(checkphonenumber.Contains))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Not a valid PhoneNumber format");
                }
                else if (!regReq.Email.Any(char.IsLower))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Email must be in Lowercase");
                }
                else if (!checkphonenumber.All(c => char.IsDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "PhoneNumber must be number");
                }
                else if (!regReq.Gender.All(c => char.IsLetter(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Gender must be alphabet");
                }
                else if (!regReq.CountryofResidence.All(c => char.IsLetter(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "CountryofResidence must be alphabet");
                }
                else
                {
                    //Log Api Request
                    await _systemUtility.JsonRequestLog(regReq);
                    // check for referalcode
                    IEnumerable<PnaSuperMember> supermemberresults;
                    IEnumerable<PnaMemberType> PnaMemberTyperesults;
                    using (IDbConnection connu = Connection)
                    {
                        string selectQuerylogon3 = @"SELECT * FROM [DB_A57DC4_pnaDb_SuperMember] where ReferralCode=@referralCode";

                        supermemberresults = await connu.QueryAsync<PnaSuperMember>(selectQuerylogon3, new
                        {
                            regReq.referralCode
                        });
                        int rowlogon3 = supermemberresults.Count();

                        if (rowlogon3 >= 1)
                        {
                            foreach (var supermemberInfo in supermemberresults)
                            {
                                if (supermemberInfo.ReferralLimit == 0)
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Referral has reach it Limit, Kindly contact the Referral");
                                }
                                else
                                {
                                    //Check if user already exist
                                    string selectQuery = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where PhoneNumbers=@PhoneNumber or Email=@Email";

                                    var userExist = await connu.QueryAsync(selectQuery, new
                                    {
                                        regReq.PhoneNumber,
                                        regReq.Email
                                    });
                                    int row = userExist.Count();
                                    if (row >= 1)
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Member already registered");
                                    }
                                    else
                                    {
                                        var CheckPassword = ValidatePassword(regReq.Password, out string ErroMessage);
                                        if (string.IsNullOrEmpty(regReq.Password))
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Password is required");
                                        }
                                        else if (CheckPassword == false)
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, $"{ErroMessage}");
                                        }
                                        else
                                        {
                                            //Register New Member
                                            //Subtract ReferralLimit from SuperMember
                                            double subtractReferralLimit = supermemberInfo.ReferralLimit - 1;
                                            //Generate Qrcode
                                            var generateQrcode = await _sharedService.GenerateQRCode(WalletId);
                                            double WalletBalance = 0;
                                            string subscriptionsStatus = "Not subscribed";
                                            bool IsActivated = false;
                                            string Status = "Inactive";
                                            var Password = Cipher.GenerateHash(regReq.Password);
                                            string sqlQuery = "INSERT INTO [DB_A57DC4_pnaDb_Member]([FullName],[PhoneNumbers],[Email],[ReferralCode],[WalletId],[Alias],[DateofBirth],[Gender],[CountryofResidence],[Profession],[TellUsAboutYourself],[WhyDoYouWantToJoin],[QRcode],[MemberTypeId],[subscriptionsStatus],[ActivationCode],[IsActivated],[Status]) VALUES(@FullName,@PhoneNumber,@Email,@ReferralCode,@WalletId,@Alias,@DateofBirth,@Gender,@CountryofResidence,@Profession,@TellUsAboutYourself,@WhyDoYouWantToJoin,@Data,@MemberTypeId,@subscriptionsStatus,@ActivationCode,@IsActivated,@Status)";
                                            var results = await connu.ExecuteAsync(sqlQuery,
                                                 new
                                                 {
                                                     regReq.FullName,
                                                     regReq.PhoneNumber,
                                                     regReq.Email,
                                                     ReferralCode,
                                                     WalletId,
                                                     regReq.Alias,
                                                     regReq.DateofBirth,
                                                     regReq.Gender,
                                                     regReq.CountryofResidence,
                                                     regReq.Profession,
                                                     regReq.TellUsAboutYourself,
                                                     regReq.WhyDoYouWantToJoin,
                                                     generateQrcode.Data,
                                                     regReq.MemberTypeId,
                                                     subscriptionsStatus,
                                                     ActivationCode,
                                                     IsActivated,
                                                     Status
                                                 });
                                            string sqlQuery2 = "INSERT INTO [DB_A57DC4_pnaDb_Wallet]([WalletId],[WalletBalance]) VALUES(@WalletId,@WalletBalance)";
                                            var results2 = await connu.ExecuteAsync(sqlQuery2,
                                                 new
                                                 {
                                                     WalletId,
                                                     WalletBalance
                                                 });
                                            string sqlQuery3 = "INSERT INTO [DB_A57DC4_pnaDb_MemberAuth]([PhoneNumber],[Email],[Password],[TokenExpiry]) VALUES(@PhoneNumber,@Email,@Password,@a)";
                                            var results3 = await connu.ExecuteAsync(sqlQuery3,
                                                 new
                                                 {
                                                     regReq.PhoneNumber,
                                                     regReq.Email,
                                                     Password,
                                                     a = logdate
                                                 });
                                            if (results >= 1)
                                            {
                                                //send activation code
                                                string Message = "Hello here is your activation code: " + ActivationCode + "";
                                                string json = File.ReadAllText(@"Resources\emp.json");
                                                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                                                jsonObj["SMS"]["message"]["messagetext"] = $"{Message}";
                                                jsonObj["SMS"]["recipients"]["gsm"][0]["msidn"] = $"{regReq.PhoneNumber}";
                                                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                                                File.WriteAllText(@"Resources\emp.json", output);

                                                string endpoint = $"{_baseUrls.SMSAPI}";
                                                var reqBody = JsonConvert.SerializeObject(jsonObj);
                                                var SmsResponse = await _httpClient.Post(reqBody, endpoint, requestId);

                                                var SmsDTO = JsonConvert.DeserializeObject<SmsResponse>(SmsResponse);
                                                if (SmsDTO.response.status == "SUCCESS")
                                                {
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Activation Code sent");

                                                    var syslog = new Sys_logReq()
                                                    {
                                                        channel = _appChannel.Channel1,
                                                        description = $"Created New Member {regReq.FullName}",
                                                        UserID = regReq.Email,
                                                        Usertype = "Member"
                                                    };
                                                    var sys_log = await _systemUtility.sys_Log(syslog);

                                                    // update supermember ReferalLimit
                                                    string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_SuperMember] SET ReferralLimit = @subtractReferralLimit WHERE ReferralCode=@referralCode";

                                                    var updateRequest = await connu.ExecuteAsync(updateQuery, new
                                                    {
                                                        subtractReferralLimit,
                                                        regReq.referralCode
                                                    });
                                                }
                                                else
                                                {
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"{SmsDTO.response.status}");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid ReferralCode");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][Create new member][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][Create new member][Response] => {ex.Message} | [request]=> " +
                                                JsonConvert.SerializeObject(regReq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> payMembershipSubcription(payMembershipSubcription payMembershipSubcription)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);

            string TransactionReference = Guid.NewGuid().ToString("N").ToUpper().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string Generatepassword = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string paymentReference = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 15);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            var logmonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
            //string WalletId = Helper.GenerateUniqueId(6);
            //double ReferralLimit = 1000;

            try
            {
                if (string.IsNullOrWhiteSpace(payMembershipSubcription.WalletId))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId is required");
                }
                else
                {
                    //step 1-- check/validate sender info with walletid

                    IEnumerable<MemberauthDTO> senderresults;

                    IEnumerable<PnaMemberType> MemberTyperesults;

                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@WalletId";

                        senderresults = await connlogon.QueryAsync<MemberauthDTO>(selectQuerylogon, new
                        {
                            payMembershipSubcription.WalletId
                        });
                        int rowlogon = senderresults.Count();
                        if (rowlogon >= 1)
                        {
                            foreach (var authinfo in senderresults)
                            {
                                //step 2 -- check Sender wallet balance
                                string selectQuerylogon3 = @"SELECT * FROM [DB_A57DC4_pnaDb_MemberType] where Id=@MemberTypeId";

                                MemberTyperesults = await connlogon.QueryAsync<PnaMemberType>(selectQuerylogon3, new
                                {
                                    authinfo.MemberTypeId
                                });
                                int rowlogon3 = MemberTyperesults.Count();

                                if (rowlogon3 >= 1)
                                {
                                    foreach (var MemberTypeInfo in MemberTyperesults)
                                    {
                                        if (authinfo.subscriptionsStatus == "subscribed")
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "You have an active Membership plan");
                                        }
                                        else
                                        {
                                            // initiate monnify
                                            string paymentDescription = "Membership Subscription";
                                            string json = File.ReadAllText(@"Resources\init_transaction.json");
                                            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                                            jsonObj["amount"] = $"{MemberTypeInfo.SubscriptionAmount}";
                                            jsonObj["customerName"] = $"{authinfo.FullName}";
                                            jsonObj["customerEmail"] = $"{authinfo.Email}";
                                            jsonObj["paymentReference"] = $"{paymentReference}";
                                            jsonObj["paymentDescription"] = $"{paymentDescription}";
                                            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                                            File.WriteAllText(@"Resources\init_transaction.json", output);

                                            string endpoint = $"{_baseUrls.monnifyUrl}merchant/transactions/init-transaction";
                                            var reqBody = JsonConvert.SerializeObject(jsonObj);
                                            var init_transactionResponse = await _httpClient.Post(reqBody, endpoint, requestId);
                                            var init_transactionDTO = JsonConvert.DeserializeObject<init_transactionDTO>(init_transactionResponse);

                                            if (init_transactionDTO.responseCode == "0")
                                            {
                                                var FundwalletDTO = new FundwalletDTO()
                                                {
                                                    checkoutUrl = init_transactionDTO.responseBody.checkoutUrl,
                                                    paymentReference = init_transactionDTO.responseBody.paymentReference
                                                };

                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, $"{init_transactionDTO.responseMessage}", FundwalletDTO);

                                                // insert payment instance to the db
                                                var FundWalletHistory = new MembershipSubscription()
                                                {
                                                    paymentReference = FundwalletDTO.paymentReference,
                                                    amount = MemberTypeInfo.SubscriptionAmount,
                                                    IsMembershipsubscribed = false,
                                                    WalletId = authinfo.WalletId
                                                };
                                                using (IDbConnection conn = Connection)
                                                {
                                                    conn.Insert(FundWalletHistory);
                                                }
                                            }
                                            else
                                            {
                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, $"{init_transactionDTO.responseMessage}");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "No MemberType attached to Member");
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid WalletId");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][payMembershipSubcription][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][payMembershipSubcription][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> Profilepictureupload(string Email, IFormFile profile_pic)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Email is Required");
                }
                else if (Path.GetExtension(profile_pic.FileName).ToLower() != ".jpg"
              && Path.GetExtension(profile_pic.FileName).ToLower() != ".png"
              && Path.GetExtension(profile_pic.FileName).ToLower() != ".jpeg")
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "profile picture  is not a valid image file type");
                }
                else if (profile_pic.FileName.Length > 0)
                {
                    IEnumerable<Userdetails> adminstaffresults;
                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where Email=@Email";

                        adminstaffresults = await connlogon.QueryAsync<Userdetails>(selectQuerylogon, new
                        {
                            Email
                        });
                        int rowlogon = adminstaffresults.Count();
                        // log into database

                        if (rowlogon >= 1)
                        {
                            foreach (var adminstaffInfo in adminstaffresults)
                            {
                                var folderName = Path.Combine("Resources", "Images");
                                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                                var fileName = ContentDispositionHeaderValue.Parse(profile_pic.ContentDisposition).FileName.Trim('"');
                                //var fileName = profile_pic.FileName;
                                var fullPath = Path.Combine(pathToSave, fileName);
                                var dbPath = Path.Combine(folderName, fileName);

                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    profile_pic.CopyTo(stream);
                                }

                                byte[] imageArray = System.IO.File.ReadAllBytes($"{fullPath}");
                                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                                var getprofilepicUrl = _baseUrls.ProfilepictureUrl + fileName;

                                using (IDbConnection connu = Connection)
                                {
                                    string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_Member] SET ProfilePicture = @getprofilepicUrl WHERE Email=@Email";

                                    var updateRequest = await connu.ExecuteAsync(updateQuery, new
                                    {
                                        getprofilepicUrl,
                                        Email
                                    });

                                    if (updateRequest >= 1)
                                    {
                                        // Log Activities
                                        var syslog = new Sys_logReq()
                                        {
                                            channel = _appChannel.Channel1,
                                            description = $"Member {adminstaffInfo.FullName} Update profile picture Successfully",
                                            UserID = adminstaffInfo.Email,
                                            Usertype = _appSystemType.Type8
                                        };
                                        var sys_log = await _systemUtility.sys_Log(syslog);

                                        var CooperatorProfilePic = new MemberProfilePic()
                                        {
                                            ProfilePic = getprofilepicUrl
                                        };
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Profile picture Successfully updated", CooperatorProfilePic);
                                    }
                                    else
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "System Down Try again");
                                    }
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid Member Account");
                        }
                    }
                }
                else
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][Profilepictureupload][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][Profilepictureupload][Response] => {ex.Message} | [request]=> " +
                                                     JsonConvert.SerializeObject(Email) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> Resend_activationcode(Resend_activationcodeReq resend_ActivationcodeReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            Random Rnd = new Random();
            string ActivationCode = (Rnd.Next(100000, 999999)).ToString();
            try
            {
                Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
                bool isValidEmail = regex.IsMatch(resend_ActivationcodeReq.Email);
                if (!isValidEmail)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "The email is invalid");
                }
                else if (!resend_ActivationcodeReq.Email.Any(char.IsLower))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Email must be in Lowercase");
                }
                else
                {
                    //Log Api Request
                    await _systemUtility.JsonRequestLog(resend_ActivationcodeReq);
                    IEnumerable<Userdetails> Userdetailsresults;
                    using (IDbConnection conn = Connection)
                    {
                        string selectQuery6 = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where Email=@Email and IsActivated='0'";

                        Userdetailsresults = await conn.QueryAsync<Userdetails>(selectQuery6, new
                        {
                            resend_ActivationcodeReq.Email
                        });
                        int row2 = Userdetailsresults.Count();
                        if (row2 >= 1)
                        {
                            foreach (var Info in Userdetailsresults)
                            {
                                //update user table
                                string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_Member] SET ActivationCode=@ActivationCode WHERE Email=@Email";

                                var updateRequest = await conn.ExecuteAsync(updateQuery, new
                                {
                                    ActivationCode,
                                    resend_ActivationcodeReq.Email
                                });

                                if (updateRequest >= 1)
                                {
                                    //send activation code
                                    string Message = $"Hello {Info.FullName} here is your activation code: " + ActivationCode + "";
                                    string json = File.ReadAllText(@"Resources\emp.json");
                                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                                    jsonObj["SMS"]["message"]["messagetext"] = $"{Message}";
                                    jsonObj["SMS"]["recipients"]["gsm"][0]["msidn"] = $"{Info.PhoneNumbers}";
                                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                                    File.WriteAllText(@"Resources\emp.json", output);

                                    string endpoint = $"{_baseUrls.SMSAPI}";
                                    var reqBody = JsonConvert.SerializeObject(jsonObj);
                                    var SmsResponse = await _httpClient.Post(reqBody, endpoint, requestId);

                                    var SmsDTO = JsonConvert.DeserializeObject<SmsResponse>(SmsResponse);
                                    if (SmsDTO.response.status == "SUCCESS")
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Activation Code sent");

                                        var syslog = new Sys_logReq()
                                        {
                                            channel = _appChannel.Channel1,
                                            description = $"Activation Code Resent for {Info.FullName}",
                                            UserID = Info.Email,
                                            Usertype = "Member"
                                        };
                                        var sys_log = await _systemUtility.sys_Log(syslog);
                                    }
                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Error");
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Is either you are not a member yet, or you have activated before");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][Resend_activationcode][Response] => {ex.Message} | [requestId]=> {requestId}");

                await _systemUtility.JsonErrorLog($"[PNAAPI][Resend_activationcode][Response] => {ex.Message} | [request]=> " +
                                                 JsonConvert.SerializeObject(resend_ActivationcodeReq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> Restpassword(string Email)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            string Generatepassword = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string Generateusername = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            try
            {
                //validation
                Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
                bool isValidEmail = regex.IsMatch(Email);
                if (string.IsNullOrWhiteSpace(Email))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Email is Required");
                }
                else if (!isValidEmail)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "The email is invalid");
                }
                else if (!Email.Any(char.IsLower))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Email must be in Lowercase");
                }
                else
                {
                    IEnumerable<Userdetails> adminstaffresults;
                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where Email=@Email";

                        adminstaffresults = await connlogon.QueryAsync<Userdetails>(selectQuerylogon, new
                        {
                            Email
                        });
                        int rowlogon = adminstaffresults.Count();
                        // log into database

                        if (rowlogon >= 1)
                        {
                            foreach (var adminstaffInfo in adminstaffresults)
                            {
                                var Password = Cipher.GenerateHash(Generatepassword);
                                using (IDbConnection connu = Connection)
                                {
                                    string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_MemberAuth] SET Password = @Password, PasswordChangeDate=@logdate WHERE Email=@Email";

                                    var updateRequest = await connu.ExecuteAsync(updateQuery, new
                                    {
                                        Password,
                                        logdate,
                                        Email
                                    });

                                    if (updateRequest >= 1)
                                    {
                                        // Log Activities
                                        var syslog = new Sys_logReq()
                                        {
                                            channel = _appChannel.Channel1,
                                            description = $"User {adminstaffInfo.FullName} Reset password Successfully",
                                            UserID = adminstaffInfo.Email,
                                            Usertype = _appSystemType.Type8
                                        };
                                        var sys_log = await _systemUtility.sys_Log(syslog);

                                        //send Email
                                        //var getmessage = $"your New password {Generatepassword}.";

                                        var subject = "Reset your password";

                                        //string fileName = "myfile.ext";
                                        string path1 = @"Resources\EmailTemplate\Resetpassword.html";

                                        // string path2 = @"\mydir";
                                        string fullPath;

                                        fullPath = Path.GetFullPath(path1);
                                        StreamReader str = new StreamReader(fullPath);
                                        string MailText = str.ReadToEnd();
                                        str.Close();
                                        MailText = MailText.Replace("[fullname]", $"{adminstaffInfo.FullName}");
                                        MailText = MailText.Replace("[email]", adminstaffInfo.Email);
                                        MailText = MailText.Replace("[newpassword]", Generatepassword);

                                        var SendEmailApi = new SendEmailApiReq()
                                        {
                                            htmlBody = MailText,
                                            MailSubject = subject,
                                            MailTo = adminstaffInfo.Email
                                        };
                                        await _notificationServices.sendmail(SendEmailApi);
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Reset password Successfully check your Mail");
                                    }
                                    else
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "System Down Try again");
                                    }
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid User Account");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[VeexcelAPI][reset password][Response] => {ex.Message} | [request]=> " +
                                                   JsonConvert.SerializeObject(Email) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> reversal_Wallet(ReversalFundReq reversalFundReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);

            string TransactionReference = Guid.NewGuid().ToString("N").ToUpper().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string Generatepassword = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string Generateusername = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            var logmonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
            //string WalletId = Helper.GenerateUniqueId(6);
            //double ReferralLimit = 1000;
            try
            {
                if (!reversalFundReq.WalletId.All(c => char.IsDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId must be a number");
                }
                else if (!reversalFundReq.TransactionPin.All(c => char.IsDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "TransactionPin must be a number");
                }
                else
                {
                    IEnumerable<PnaMember> senderresults;
                    IEnumerable<WalletDTO> Senderwalletresults;

                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@WalletId";

                        senderresults = await connlogon.QueryAsync<PnaMember>(selectQuerylogon, new
                        {
                            reversalFundReq.WalletId
                        });
                        int rowlogon = senderresults.Count();
                        if (rowlogon >= 1)
                        {
                            foreach (var authinfo in senderresults)
                            {
                                //step 2 -- check Sender wallet balance
                                string selectQuerylogon3 = @"SELECT * FROM [DB_A57DC4_pnaDb_Wallet] where WalletId=@WalletId";

                                Senderwalletresults = await connlogon.QueryAsync<WalletDTO>(selectQuerylogon3, new
                                {
                                    reversalFundReq.WalletId
                                });
                                int rowlogon3 = Senderwalletresults.Count();

                                if (rowlogon3 >= 1)
                                {
                                    foreach (var WalletInfo in Senderwalletresults)
                                    {
                                        //step 5--- credit  amount from sender wallet
                                        double credit = WalletInfo.WalletBalance + reversalFundReq.amount;
                                        //step 6-- - update sender wallet balance
                                        string updateQuerybc = @"UPDATE [DB_A57DC4_pnaDb_Wallet] SET WalletBalance = @credit WHERE WalletId=@WalletId";

                                        var updatesenderwalletbalance = await connlogon.ExecuteAsync(updateQuerybc, new
                                        {
                                            credit,

                                            reversalFundReq.WalletId
                                        });
                                        if (updatesenderwalletbalance >= 1)
                                        {
                                            var debit_WalletDTO = new debit_WalletDTO()
                                            {
                                                IsTransaction_status = "SUCCESS",
                                                Wallet_balance = credit
                                            };
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Ok", debit_WalletDTO);
                                        }
                                        else
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Could not process reversal_Wallet Request. Please retry in a moment");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid User WalletId");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][reversal_Wallet][Response] => {ex.Message} | [requestId]=> {requestId}");

                await _systemUtility.JsonErrorLog($"[PNAAPI][reversal_Wallet][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> SetupTransactionpin(TransactionPinSetupReq transactionPinSetupReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrWhiteSpace(transactionPinSetupReq.WalletId))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId is required");
                }
                else if (transactionPinSetupReq.New_TransactionPin.ToString().Length > 4)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Transaction Pin must not be more than 4 digits");
                }
                else if (transactionPinSetupReq.New_TransactionPin.ToString().Length < 4)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Transaction Pin must be 4 digits");
                }
                else if (!transactionPinSetupReq.New_TransactionPin.All(c => char.IsNumber(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Transaction Pin must be a number");
                }
                else if (!transactionPinSetupReq.WalletId.All(c => char.IsNumber(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId must not be a number");
                }
                else
                {
                    //check for Member

                    IEnumerable<Userdetails> userauthresults;
                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@WalletId";

                        userauthresults = await connlogon.QueryAsync<Userdetails>(selectQuerylogon, new
                        {
                            transactionPinSetupReq.WalletId
                        });
                        int rowlogon = userauthresults.Count();
                        // log into database
                        if (rowlogon >= 1)
                        {
                            foreach (var authinfo in userauthresults)
                            {
                                if (authinfo.IsActivated == false)
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Kindly Setup your mobile app");
                                }
                                else if (authinfo.TransactionPinSetupSatus == true)
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Transaction Pin is set already");
                                }
                                else
                                {
                                    var TransactionPin = Cipher.GenerateHash(transactionPinSetupReq.New_TransactionPin);
                                    bool TransactionPinSetupSatus = true;
                                    string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_Member] SET TransactionPin = @TransactionPin,TransactionPinSetupSatus=@TransactionPinSetupSatus WHERE WalletId = @WalletId";

                                    var updateRequest = await connlogon.ExecuteAsync(updateQuery, new
                                    {
                                        TransactionPin,
                                        TransactionPinSetupSatus,
                                        transactionPinSetupReq.WalletId
                                    });
                                    if (updateRequest >= 1)
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Transaction Pin setup successfully");
                                        // Log Activities
                                        var syslog = new Sys_logReq()
                                        {
                                            channel = _appChannel.Channel1,
                                            description = $"User {authinfo.FullName} Transaction Pin setup successfully",
                                            UserID = authinfo.Email,
                                            Usertype = _appSystemType.Type8
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
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "OOps Nothing found for you");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][setup transaction pin][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][setup transaction pin][Response] => {ex.Message} | [request]=> " +
                                                  $"{transactionPinSetupReq} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> TransferWalletToWallet(Wallet2WalletTransferReq wallet2WalletTransferReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            string PaymentMethod = "Wallet Transfer";

            string TransactionReference = Guid.NewGuid().ToString("N").ToUpper().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string Generatepassword = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            string Generateusername = Guid.NewGuid().ToString("N").ToLower().Replace("1", "").Replace("o", "").Replace("0", "").Substring(0, 8);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            var logmonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
            //string WalletId = Helper.GenerateUniqueId(6);
            //double ReferralLimit = 1000;

            try
            {
                if (string.IsNullOrWhiteSpace(wallet2WalletTransferReq.SenderWalletId))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "SenderWalletId is required");
                }
                else if (string.IsNullOrWhiteSpace(wallet2WalletTransferReq.ReceiverWalletId))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "RecieverWalletId is required");
                }
                else if (string.IsNullOrWhiteSpace(wallet2WalletTransferReq.Narration))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Narration is required");
                }
                else
                {
                    //step 1-- check/validate sender info with walletid

                    IEnumerable<Userdetails> senderresults;
                    IEnumerable<Userdetails> recieverresults;
                    IEnumerable<WalletDTO> Senderwalletresults;
                    IEnumerable<WalletDTO> Recieverwalletresults;
                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@SenderWalletId";

                        senderresults = await connlogon.QueryAsync<Userdetails>(selectQuerylogon, new
                        {
                            wallet2WalletTransferReq.SenderWalletId
                        });
                        int rowlogon = senderresults.Count();
                        // log into database
                        if (rowlogon >= 1)
                        {
                            foreach (var authinfo in senderresults)
                            {
                                //step 2 -- check Sender wallet balance
                                string selectQuerylogon3 = @"SELECT * FROM [DB_A57DC4_pnaDb_Wallet] where WalletId=@SenderWalletId";

                                Senderwalletresults = await connlogon.QueryAsync<WalletDTO>(selectQuerylogon3, new
                                {
                                    wallet2WalletTransferReq.SenderWalletId
                                });
                                int rowlogon3 = Senderwalletresults.Count();
                                string selectQuerylogon3w = @"SELECT * FROM [DB_A57DC4_pnaDb_Wallet] where WalletId=@ReceiverWalletId";

                                Recieverwalletresults = await connlogon.QueryAsync<WalletDTO>(selectQuerylogon3w, new
                                {
                                    wallet2WalletTransferReq.ReceiverWalletId
                                });
                                int rowlogon3w = Recieverwalletresults.Count();
                                if (rowlogon3 >= 1)
                                {
                                    foreach (var WalletInfo in Senderwalletresults)
                                    {
                                        //step 3-- validate transfer amount against wallet balance

                                        if (wallet2WalletTransferReq.Amount > WalletInfo.WalletBalance)
                                        {
                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INSUFFICIENT_BAL, "INSUFFICIENT BALANCE");
                                        }
                                        else
                                        {
                                            //step 4-- check / validate reciever info with walletid
                                            string selectQuerylogon4 = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where WalletId=@ReceiverWalletId";

                                            recieverresults = await connlogon.QueryAsync<Userdetails>(selectQuerylogon4, new
                                            {
                                                wallet2WalletTransferReq.ReceiverWalletId
                                            });
                                            int rowlogon4 = recieverresults.Count();
                                            if (rowlogon4 >= 1)
                                            {
                                                //step 5-- - check / validate sender transaction pin
                                                var transactionpin = Cipher.GenerateHash(wallet2WalletTransferReq.TransactionPin.ToString());
                                                if (transactionpin != authinfo.TransactionPin)
                                                {
                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid Transaction Pin, Kindly Retry");
                                                }
                                                else
                                                {
                                                    foreach (var ReceiverInfow in recieverresults)
                                                    {
                                                        if (ReceiverInfow.FullName == authinfo.FullName)
                                                        {
                                                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "you can not transfer fund to self.");
                                                        }
                                                        else
                                                        {
                                                            foreach (var WalletInfow in Recieverwalletresults)
                                                            {
                                                                //step 6--- debit transfer amount from sender wallet
                                                                double debit = WalletInfo.WalletBalance - wallet2WalletTransferReq.Amount;
                                                                string TransactionNarration = $"Transfer to {ReceiverInfow.FullName}";
                                                                //step 7-- credit transfer amount to reciever wallet
                                                                double credit = WalletInfow.WalletBalance + wallet2WalletTransferReq.Amount;
                                                                //step 8-- - update sender wallet balance
                                                                string updateQuerybc = @"UPDATE [DB_A57DC4_pnaDb_Wallet] SET WalletBalance = @debit WHERE WalletId=@SenderWalletId";

                                                                var updatesenderwalletbalance = await connlogon.ExecuteAsync(updateQuerybc, new
                                                                {
                                                                    debit,

                                                                    wallet2WalletTransferReq.SenderWalletId
                                                                });
                                                                //step 9-- update reciever wallet balance
                                                                string updateQuerybcw = @"UPDATE [DB_A57DC4_pnaDb_Wallet] SET WalletBalance = @credit WHERE WalletId=@ReceiverWalletId";

                                                                var updaterecieverwalletbalance = await connlogon.ExecuteAsync(updateQuerybcw, new
                                                                {
                                                                    credit,

                                                                    wallet2WalletTransferReq.ReceiverWalletId
                                                                });
                                                                if (updatesenderwalletbalance >= 1 && updaterecieverwalletbalance >= 1)
                                                                {
                                                                    // step 10-- transaction status payload
                                                                    var WalletTransferDetails = new WalletTransferDetails()
                                                                    {
                                                                        SenderFullName = authinfo.FullName,
                                                                        SenderPhone = authinfo.PhoneNumbers,
                                                                        SenderWalletId = authinfo.WalletId,
                                                                        SenderEmail = authinfo.Email,
                                                                        ReceiverFullName = ReceiverInfow.FullName,
                                                                        ReceiverPhone = ReceiverInfow.PhoneNumbers,
                                                                        ReceiverWalletId = ReceiverInfow.WalletId,
                                                                        Amount = wallet2WalletTransferReq.Amount,
                                                                        Narration = TransactionNarration,
                                                                        TransactionReference = TransactionReference,
                                                                        TransactionDate = logdate,
                                                                        Transactionstatus = "Success",
                                                                        TransactionType = PaymentMethod
                                                                    };
                                                                    var WalletTransferHistoryDR = new WalletTransferHistory()
                                                                    {
                                                                        Amount = WalletTransferDetails.Amount,
                                                                        Narration = WalletTransferDetails.Narration,
                                                                        ReceiverFullName = WalletTransferDetails.ReceiverFullName,
                                                                        ReceiverPhone = WalletTransferDetails.ReceiverPhone,
                                                                        ReceiverWalletId = "XXXXXXX",
                                                                        SenderEmail = WalletTransferDetails.SenderEmail,
                                                                        SenderFullName = WalletTransferDetails.SenderFullName,
                                                                        SenderPhone = WalletTransferDetails.SenderPhone,
                                                                        SenderWalletId = WalletTransferDetails.SenderWalletId,
                                                                        TransactionDate = WalletTransferDetails.TransactionDate,
                                                                        TransactionReference = WalletTransferDetails.TransactionReference,
                                                                        Transactionstatus = WalletTransferDetails.Transactionstatus,
                                                                        TransactionType = "Dr"
                                                                    };
                                                                    var WalletTransferHistoryCR = new WalletTransferHistory()
                                                                    {
                                                                        Amount = WalletTransferDetails.Amount,
                                                                        Narration = WalletTransferDetails.Narration,
                                                                        ReceiverFullName = WalletTransferDetails.ReceiverFullName,
                                                                        ReceiverPhone = WalletTransferDetails.ReceiverPhone,
                                                                        ReceiverWalletId = WalletTransferDetails.ReceiverWalletId,
                                                                        SenderEmail = WalletTransferDetails.SenderEmail,
                                                                        SenderFullName = WalletTransferDetails.SenderFullName,
                                                                        SenderPhone = WalletTransferDetails.SenderPhone,
                                                                        SenderWalletId = "XXXXXXXX",
                                                                        TransactionDate = WalletTransferDetails.TransactionDate,
                                                                        TransactionReference = WalletTransferDetails.TransactionReference,
                                                                        Transactionstatus = WalletTransferDetails.Transactionstatus,
                                                                        TransactionType = "Cr"
                                                                    };
                                                                    using (IDbConnection conn = Connection)
                                                                    {
                                                                        conn.Insert(WalletTransferHistoryCR);
                                                                    }
                                                                    using (IDbConnection conn = Connection)
                                                                    {
                                                                        conn.Insert(WalletTransferHistoryDR);
                                                                    }
                                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Transaction Info", WalletTransferDetails);
                                                                    // send sms
                                                                    // step 11-- send sms to both(Dr, Cr)
                                                                    string Drmsg = $"Your Wallet Account Has Been Debited with N{wallet2WalletTransferReq.Amount.ToString("N", CultureInfo.CurrentCulture)} on {logdate.ToString("G", DateTimeFormatInfo.InvariantInfo)} By PWT:PNA App/{ReceiverInfow.FullName}";
                                                                    string Crmsg = $"Your Wallet Account Has Been Credited With  N{wallet2WalletTransferReq.Amount.ToString("N", CultureInfo.CurrentCulture)} on {logdate.ToString("G", DateTimeFormatInfo.InvariantInfo)} By PWT:PNA App/{authinfo.FullName}";
                                                                    //var Drsms = await _notificationServices.SendSMS(authinfo.PhoneNumber,Drmsg);
                                                                    string json = File.ReadAllText(@"Resources\emp.json");
                                                                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                                                                    jsonObj["SMS"]["message"]["messagetext"] = $"{Drmsg}";
                                                                    jsonObj["SMS"]["recipients"]["gsm"][0]["msidn"] = $"{authinfo.PhoneNumbers}";
                                                                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                                                                    File.WriteAllText(@"Resources\emp.json", output);

                                                                    string endpoint = $"{_baseUrls.SMSAPI}";
                                                                    var reqBody = JsonConvert.SerializeObject(jsonObj);
                                                                    var SmsResponse = await _httpClient.Post(reqBody, endpoint, requestId);
                                                                    //DR
                                                                    // set asterisk to hide first n - 4 digits
                                                                    string asterisks = new string('*', wallet2WalletTransferReq.SenderWalletId.Length - 4);
                                                                    string asterisks2 = new string('*', wallet2WalletTransferReq.ReceiverWalletId.Length - 4);

                                                                    // pick last 4 digits for showing
                                                                    string last = wallet2WalletTransferReq.SenderWalletId.Substring(wallet2WalletTransferReq.SenderWalletId.Length - 4, 4);
                                                                    string last2 = wallet2WalletTransferReq.ReceiverWalletId.Substring(wallet2WalletTransferReq.ReceiverWalletId.Length - 4, 4);
                                                                    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                                                                    // combine both asterisk mask and last digits
                                                                    string result = asterisks + last;
                                                                    string result2 = asterisks2 + last2;
                                                                    var subject1 = $"Debit Transaction Alert on {result}";
                                                                    var subject2 = $"Credit Transaction Alert on {result2}";

                                                                    //MailText = MailText.Replace("[transferamount]", contributionReq.Amount.ToString("N", nfi));
                                                                    var transferamount = wallet2WalletTransferReq.Amount.ToString("N", nfi);
                                                                    //MailText2 = MailText2.Replace("[transferamount]", contributionReq.Amount.ToString("N", nfi));
                                                                    // MailText = MailText.Replace("[transactiontype]", "DR");
                                                                    var transactiontype1 = "DR";
                                                                    var transactiontype2 = "CR";

                                                                    // MailText2 = MailText2.Replace("[transactiontype]", "CR");
                                                                    // MailText = MailText.Replace("[getwalletid]", result);
                                                                    var getwalletid = result;
                                                                    var getwalletid2 = result2;
                                                                    //MailText2 = MailText2.Replace("[getwalletid2]", result2);
                                                                    // MailText2 = MailText2.Replace("[getRwalletid2]", result2);
                                                                    // MailText = MailText.Replace("[transtype]", "Wallet Transfer");
                                                                    //MailText2 = MailText2.Replace("[transtype]", "Wallet Transfer");
                                                                    var transtype = "Wallet Transfer";
                                                                    // MailText = MailText.Replace("[getRwalletid]", result2);
                                                                    // MailText = MailText.Replace("[fullname]", authinfo.FirstName + " " + authinfo.LastName);
                                                                    var fullname = authinfo.FullName;
                                                                    var Rfullname = ReceiverInfow.FullName;
                                                                    //MailText2 = MailText2.Replace("[fullname2]", ReceiverInfow.CooperativeName);
                                                                    //MailText2 = MailText2.Replace("[Rfullname2]", ReceiverInfow.CooperativeName);
                                                                    //MailText = MailText.Replace("[Rfullname]", ReceiverInfow.CooperativeName);
                                                                    //MailText2 = MailText2.Replace("[narration]", TransactionNarration + " " + "On" + " " + contributionReq.Narration);
                                                                    var narrationCR = TransactionNarration + " " + "for" + " " + wallet2WalletTransferReq.Narration;
                                                                    var narrationDR = wallet2WalletTransferReq.Narration;

                                                                    //MailText = MailText.Replace("[narration]", TransactionNarration + " " + "for" + " " + contributionReq.Narration);
                                                                    //MailText = MailText.Replace("[transactionref]", TransactionReference);
                                                                    var transactionref = TransactionReference;
                                                                    //MailText2 = MailText2.Replace("[transactionref]", TransactionReference);
                                                                    // MailText = MailText.Replace("[transactiondate]", logdate.ToString("F", CultureInfo.CreateSpecificCulture("en-US")));
                                                                    //MailText2 = MailText2.Replace("[transactiondate]", logdate.ToString("F", CultureInfo.CreateSpecificCulture("en-US")));
                                                                    var transactiondate = logdate.ToString("F", CultureInfo.CreateSpecificCulture("en-US"));

                                                                    //await _notificationServices.sendmail(SendEmailApi);
                                                                    // var Crsms = await _notificationServices.SendSMS(ReceiverInfow.PhoneNumber, Crmsg);
                                                                    string json2 = File.ReadAllText(@"Resources\emp.json");
                                                                    dynamic jsonObj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(json2);
                                                                    jsonObj2["SMS"]["message"]["messagetext"] = $"{Crmsg}";
                                                                    jsonObj2["SMS"]["recipients"]["gsm"][0]["msidn"] = $"{ReceiverInfow.PhoneNumbers}";
                                                                    string output2 = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj2, Newtonsoft.Json.Formatting.Indented);

                                                                    File.WriteAllText(@"Resources\emp.json", output2);

                                                                    string endpoint2 = $"{_baseUrls.SMSAPI}";
                                                                    var reqBody2 = JsonConvert.SerializeObject(jsonObj2);
                                                                    var SmsResponse2 = await _httpClient.Post(reqBody2, endpoint2, requestId);
                                                                    //CR

                                                                    //await _notificationServices.sendmail(Cremail);
                                                                    //New Implementation

                                                                    var CRTransactionEmailDTO = new TransactionEmailDTO()
                                                                    {
                                                                        result = result,
                                                                        result2 = result2,
                                                                        REmail = ReceiverInfow.Email,
                                                                        transferamount = transferamount,
                                                                        transactiontype = transactiontype2,
                                                                        getRwalletid = getwalletid2,
                                                                        getwalletid2 = getwalletid,
                                                                        transtype = transtype,
                                                                        fullname = fullname,
                                                                        Rfullname = Rfullname,
                                                                        transactionref = transactionref,
                                                                        transactiondate = transactiondate,
                                                                        narration = narrationCR
                                                                    };
                                                                    await _transactionNotification.CR(CRTransactionEmailDTO);
                                                                    var DRTransactionEmailDTO = new TransactionEmailDTO()
                                                                    {
                                                                        result = result,
                                                                        result2 = result2,
                                                                        SEmail = authinfo.Email,
                                                                        transferamount = transferamount,
                                                                        transactiontype = transactiontype1,
                                                                        getRwalletid = getwalletid2,
                                                                        getwalletid2 = getwalletid,
                                                                        transtype = transtype,
                                                                        fullname = fullname,
                                                                        Rfullname = Rfullname,
                                                                        transactionref = transactionref,
                                                                        transactiondate = transactiondate,
                                                                        narration = narrationDR
                                                                    };
                                                                    await _transactionNotification.DR(DRTransactionEmailDTO);
                                                                }
                                                                else
                                                                {
                                                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Could not process Request. Please retry in a moment");
                                                                }
                                                                //WalletTransferDetails
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid Reciever WalletId");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId does not have wallet balance");
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Invalid WalletId");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][TransferWalletToWallet][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][TransferWalletToWallet][Response] => {ex.Message} | [request]=> " +
                                                 JsonConvert.SerializeObject(wallet2WalletTransferReq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> UpdateAccountprofile(UpdateAccountprofileReq updateAccountprofileReq)
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
            Random Rnd = new Random();
            string ActivationCode = (Rnd.Next(100000, 999999)).ToString();
            string checkphonenumber = updateAccountprofileReq.PhoneNumber;
            string[] checkphionenumberarray = { "0803", "081", "0703", "0706", "0706", "0806", "0810", "0813", "0814", "0816", "0903", "0906", "0705", "0805", "0807", "0811", "0815", "0905", "0701", "0802", "0708", "0808", "0812", "0907", "0902", "0809", "0901", "0817", "0818", "0909" };
            try
            {
                //validation
                Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
                bool isValidEmail = regex.IsMatch(updateAccountprofileReq.Email);
                if (string.IsNullOrEmpty(updateAccountprofileReq.CountryofResidence))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "CountryofResidence is required");
                }
                else if (string.IsNullOrEmpty(updateAccountprofileReq.DateofBirth))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "DateofBirth is required");
                }
                else if (string.IsNullOrEmpty(updateAccountprofileReq.Email))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Email is required");
                }
                else if (string.IsNullOrEmpty(updateAccountprofileReq.FullName))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "FullName is required");
                }
                else if (string.IsNullOrEmpty(updateAccountprofileReq.Gender))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Gender is required");
                }
                else if (string.IsNullOrEmpty(updateAccountprofileReq.MembershipID))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "MembershipID is required");
                }
                else if (string.IsNullOrEmpty(updateAccountprofileReq.PhoneNumber))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "PhoneNumber is required");
                }
                else if (string.IsNullOrEmpty(updateAccountprofileReq.Profession))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Profession is required");
                }
                else if (string.IsNullOrEmpty(updateAccountprofileReq.TellUsAboutYourself))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "TellUsAboutYourself is required");
                }
                else if (string.IsNullOrEmpty(updateAccountprofileReq.Title))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.INVALID_ENTRY, "Title is required");
                }
                else if (updateAccountprofileReq.PhoneNumber.Length > 11)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "PhoneNumber must not be more than 11 digits");
                }
                else if (updateAccountprofileReq.PhoneNumber.Length < 11)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "PhoneNumber must not be less than 11 digits");
                }
                else if (!isValidEmail)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "The email is invalid");
                }
                else if (!checkphionenumberarray.Any(checkphonenumber.Contains))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Not a valid PhoneNumber format");
                }
                else if (!updateAccountprofileReq.Email.Any(char.IsLower))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Email must be in Lowercase");
                }
                else if (!checkphonenumber.All(c => char.IsDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "PhoneNumber must be number");
                }
                else if (!updateAccountprofileReq.Gender.All(c => char.IsLetter(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Gender must be alphabet");
                }
                else if (!updateAccountprofileReq.CountryofResidence.All(c => char.IsLetter(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "CountryofResidence must be alphabet");
                }
                else
                {
                    //Log Api Request
                    await _systemUtility.JsonRequestLog(updateAccountprofileReq);
                    IEnumerable<Userdetails> Userdetailsresults;
                    IEnumerable<MemberAuthResponse> MemberAuthresults;
                    using (IDbConnection conn = Connection)
                    {
                        string selectQuery6 = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where MembershipID=@MembershipID";

                        Userdetailsresults = await conn.QueryAsync<Userdetails>(selectQuery6, new
                        {
                            updateAccountprofileReq.MembershipID,
                        });
                        int row2 = Userdetailsresults.Count();
                        string selectQuery7 = @"SELECT * FROM [DB_A57DC4_pnaDb_MemberAuth] where MembershipID=@MembershipID";

                        MemberAuthresults = await conn.QueryAsync<MemberAuthResponse>(selectQuery7, new
                        {
                            updateAccountprofileReq.MembershipID,
                        });
                        int row3 = Userdetailsresults.Count();
                        if (row2 >= 1 && row3 >= 1)
                        {
                            foreach (var Info in Userdetailsresults)
                            {
                                foreach (var AuthInfo in MemberAuthresults)
                                {
                                    //update Member Info Table

                                    var memberid = Info.Id;
                                    var PnaMemberInfo = conn.Get<PnaMember>(memberid);
                                    PnaMemberInfo.Title = updateAccountprofileReq.Title;
                                    PnaMemberInfo.TellUsAboutYourself = updateAccountprofileReq.TellUsAboutYourself;
                                    PnaMemberInfo.CountryofResidence = updateAccountprofileReq.CountryofResidence;
                                    PnaMemberInfo.DateofBirth = updateAccountprofileReq.DateofBirth;
                                    PnaMemberInfo.Email = updateAccountprofileReq.Email;
                                    PnaMemberInfo.FullName = updateAccountprofileReq.FullName;
                                    PnaMemberInfo.Gender = updateAccountprofileReq.Gender;
                                    PnaMemberInfo.PhoneNumbers = updateAccountprofileReq.PhoneNumber;
                                    PnaMemberInfo.Profession = updateAccountprofileReq.Profession;
                                    PnaMemberInfo.WhyDoYouWantToJoin = updateAccountprofileReq.WhyDoYouWantToJoin;

                                    var checkquery = conn.Update(PnaMemberInfo);
                                    //update Member Auth Table
                                    var memberauthid = AuthInfo.Id;
                                    var memberauth = conn.Get<PnaMemberAuth>(memberauthid);
                                    memberauth.PhoneNumber = updateAccountprofileReq.PhoneNumber;
                                    memberauth.Email = updateAccountprofileReq.Email;
                                    var checkquery2 = conn.Update(memberauth);
                                    if (checkquery == true && checkquery2 == true)
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Profile Updated successfully");

                                        var syslog = new Sys_logReq()
                                        {
                                            channel = _appChannel.Channel1,
                                            description = $"{Info.FullName} updated profile",
                                            UserID = Info.Email,
                                            Usertype = "Member"
                                        };
                                        var sys_log = await _systemUtility.sys_Log(syslog);
                                    }
                                    else
                                    {
                                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Error updating profile,try again");
                                    }
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Not a member");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][UpdateAccountprofile][Response] => {ex.Message} | [requestId]=> {requestId}");

                await _systemUtility.JsonErrorLog($"[PNAAPI][UpdateAccountprofile][Response] => {ex.Message} | [request]=> " +
                                                 JsonConvert.SerializeObject(updateAccountprofileReq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public Task<ResponseParam> UpdateAccountsecurity(UpdateBuyerAccountsecurityReq updateAccountsecurityReq)
        {
            throw new NotImplementedException();
        }

        public bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one lower case letter.";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one upper case letter.";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password should not be lesser than 8 or greater than 15 characters.";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one numeric value.";
                return false;
            }
            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one special case character.";
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<ResponseParam> Validate_activationcode(GetActivationcodeReq getActivationcodeReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
                bool isValidEmail = regex.IsMatch(getActivationcodeReq.Email);
                if (!isValidEmail)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "The email is invalid");
                }
                else if (!getActivationcodeReq.Email.Any(char.IsLower))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Email must be in Lowercase");
                }
                else if (!getActivationcodeReq.Activationcode.All(c => char.IsDigit(c)))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Activationcode must be number");
                }
                else
                {
                    //Log Api Request
                    await _systemUtility.JsonRequestLog(getActivationcodeReq);
                    IEnumerable<Userdetails> Userdetailsresults;
                    using (IDbConnection conn = Connection)
                    {
                        string selectQuery6 = @"SELECT * FROM [DB_A57DC4_pnaDb_Member] where ActivationCode=@Activationcode and Email=@Email and IsActivated='0'";

                        Userdetailsresults = await conn.QueryAsync<Userdetails>(selectQuery6, new
                        {
                            getActivationcodeReq.Activationcode,
                            getActivationcodeReq.Email
                        });
                        int row2 = Userdetailsresults.Count();
                        if (row2 >= 1)
                        {
                            foreach (var Info in Userdetailsresults)
                            {
                                //update user table
                                string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_Member] SET IsActivated=@IsActivated,Status=@Status WHERE Email=@Email";

                                var updateRequest = await conn.ExecuteAsync(updateQuery, new
                                {
                                    IsActivated = true,
                                    Status = "Active",
                                    getActivationcodeReq.Email
                                });

                                if (updateRequest >= 1)
                                {
                                    // send welcome email to user
                                    var subject = "Welcome to Play Network Africa";
                                    string path1 = @"Resources\EmailTemplate\Welcome.html";
                                    string fullPath;
                                    fullPath = Path.GetFullPath(path1);
                                    StreamReader str = new StreamReader(fullPath);
                                    string MailText = str.ReadToEnd();
                                    str.Close();
                                    MailText = MailText.Replace("[fname]", Info.FullName.ToUpper());
                                    var SendEmailApid = new SendEmailApiReq()
                                    {
                                        htmlBody = MailText,
                                        MailSubject = subject,
                                        MailTo = getActivationcodeReq.Email
                                    };

                                    await _notificationServices.sendmail(SendEmailApid);
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "SUCCESS");
                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Validate_activationcode Failed Try again in a moment");
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Nothing found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][Validate Activation code][Response] => {ex.Message} | [requestId]=> {requestId}");

                await _systemUtility.JsonErrorLog($"[PNAAPI][Validate Activation code][Response] => {ex.Message} | [request]=> " +
                                                 JsonConvert.SerializeObject(getActivationcodeReq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> WalletNameEnquiry(WalletNameEnqReq walletNameEnqReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (string.IsNullOrWhiteSpace(walletNameEnqReq.WalletId))
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "WalletId is required");
                }
                else
                {
                    //check for Cooperator

                    IEnumerable<WalletNameEnqDTO> userauthresults;
                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT FullName FROM [DB_A57DC4_pnaDb_Member] where WalletId=@WalletId";

                        userauthresults = await connlogon.QueryAsync<WalletNameEnqDTO>(selectQuerylogon, new
                        {
                            walletNameEnqReq.WalletId
                        });
                        int rowlogon = userauthresults.Count();
                        // log into database
                        if (rowlogon >= 1)
                        {
                            foreach (var authinfo in userauthresults)
                            {
                                // check for mobile setup
                                var WalletNameEnqDTO = new WalletNameEnqDTO()
                                {
                                    FullName = authinfo.FullName
                                };
                                response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, $"ok", WalletNameEnqDTO);
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "OOps Nothing found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNAAPI][WalletNameEnquiry][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemUtility.JsonErrorLog($"[PNAAPI][WalletNameEnquiry][Response] => {ex.Message} | [Loginrequest]=> " +
                                                  JsonConvert.SerializeObject(walletNameEnqReq) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> WalletTransactionHistory(WalletTransactionHistory walletTransactionHistory)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                //query database

                string sql = "SELECT * FROM DB_A57DC4_pnaDb_Member WHERE WalletId = @WalletId; SELECT * FROM FundWalletHistory WHERE WalletId = @WalletId;SELECT * FROM WalletTransferHistory WHERE SenderWalletId = @WalletId or ReceiverWalletId=@WalletId;";
                using (var connection = Connection)
                {
                    connection.Open();
                    using (var multi = connection.QueryMultiple(sql, new { WalletId = walletTransactionHistory.WalletId }))
                    {
                        var UsersDTO = multi.ReadSingle<UsersDTO>();
                        var fundwalletDetails = multi.Read<FundwalletDetails>().ToList();
                        var WalletTransferHistoryDTO = multi.Read<WalletTransferHistoryDTO>().ToList();
                        //var GetClusterSavingsDetails = multi.Read<ClusterSavingsDetails>().ToList();
                        UsersDTO.fundwalletHistory = new List<FundwalletDetails>();
                        UsersDTO.walletTransferHistory = new List<WalletTransferHistoryDTO>();
                        // ClusterMember.ClusterSavings = new List<ClusterSavingsDetails>();
                        UsersDTO.fundwalletHistory.AddRange(fundwalletDetails);
                        UsersDTO.walletTransferHistory.AddRange(WalletTransferHistoryDTO);
                        //ClusterMember.ClusterSavings.AddRange(GetClusterSavingsDetails);
                        response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "User Info", UsersDTO);
                    }
                    //
                }
            }
            catch (Exception ex)
            {
                await _systemUtility.JsonErrorLog($"[PNAAPI][WalletTransactionHistory][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }
    }
}