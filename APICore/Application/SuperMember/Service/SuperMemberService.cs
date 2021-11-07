using APICore.Application.SuperMember.Interface;
using Dapper;
using Domain.Application.SuperMember.DTO;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;
using Utilities.SystemActivity.Interface;

namespace APICore.Application.SuperMember.Service
{
    public class SuperMemberService : ISuperMemberService
    {
        private readonly Logger _logger;
        private readonly AppKeys _appKeys;
        private readonly IConfiguration _config;
        private readonly ISystemActivityService _systemUtility;
        private readonly AppSystemType _appSystemType;
        private readonly AppChannel _appChannel;
        public SuperMemberService(Logger logger, IOptions<AppKeys> appKeys, IConfiguration configuration, ISystemActivityService systemUtility, IOptions<AppSystemType> appsystemtype, IOptions<AppChannel> channel)
        {
            _appKeys = appKeys.Value;
            _logger = logger;
            _config = configuration;
            _systemUtility = systemUtility;
            _appSystemType = appsystemtype.Value;
            _appChannel = channel.Value;

        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DB_A57DC4_PNADb"));
            }
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
                else
                {

                    IEnumerable<userAuthTableResponse> userresults;
                    IEnumerable<SuperMemberAuthResponse> userauthresults;
                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [dbo].[DB_A57DC4_pnaDb_SuperMemberAuth] where Token=@Logintoken";


                        userauthresults = await connlogon.QueryAsync<SuperMemberAuthResponse>(selectQuerylogon, new
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
                                        string selectQuery = @"SELECT * FROM [DB_A57DC4_pnaDb_SuperMember] where PhoneNumber=@PhoneNumber";

                                        userresults = await conn.QueryAsync<userAuthTableResponse>(selectQuery, new
                                        {
                                            authinfo.PhoneNumber
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
                

            }
            catch (Exception ex)
            {
                _logger.LogError($"[MM][LoginToken][Response] => {ex.Message} | [Loginrequest]=> " +
                                                  $"{Logintoken}  | [requestId]=> {requestId}");

                return new UResponseHandler().HandleException(requestId);
            }
            return response;
        }

        public async Task<ResponseParam> Login(SupermemberLoginReq loginReq)
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
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Phone field is required");
                }
                else
                {

                    // encrypt password field
                    var Password = Cipher.GenerateHash(loginReq.password);
                    var PhoneNumber = loginReq.phonemail;
                    // check the database table 
                    using (IDbConnection connlogon = Connection)
                    {
                        string selectQuerylogon = @"SELECT * FROM [dbo].[DB_A57DC4_pnaDb_SuperMemberAuth] where (PhoneNumber=@PhoneNumber or Email=@PhoneNumber)  and Password=@Password";

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

                                string updateQuery = @"UPDATE [DB_A57DC4_pnaDb_SuperMemberAuth] SET Token = @token,TokenExpiry=@TokenExpiry WHERE PhoneNumber = @PhoneNumber or Email=@PhoneNumber";

                                var updateRequest = await conn.ExecuteAsync(updateQuery, new
                                {
                                    token,
                                    TokenExpiry,
                                    PhoneNumber

                                });

                                if (updateRequest >= 1)
                                {

                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.SUCCESS, "Token", token);

                                }
                                else
                                {
                                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Unable to Generate Token");
                                }
                            }
                        }
                        else
                        {
                            response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Invalid Login details or not yet Verified, Kindly Retry");

                        }

                    }
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PNA][Login][Response] => {ex.Message} | [Loginrequest]=> " +
                                    $"{loginReq}  | [requestId]=> {requestId}");



                return new UResponseHandler().HandleException(requestId);
            }
            return response;
        }
    }

}
