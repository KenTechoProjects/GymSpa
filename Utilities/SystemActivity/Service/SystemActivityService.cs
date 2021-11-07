using Dapper;
using Domain.Application.SystemUtility.Sys_Logs;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Utilities.Binders;
using Utilities.SystemActivity.Interface;

namespace Utilities.SystemActivity.Service
{
    public class SystemActivityService : ISystemActivityService
    {
        private readonly Logger _logger;
        private readonly IConfiguration _config;
        private readonly AppSystemType _appSystemType;
        private readonly AppChannel _appChannel;

        public SystemActivityService(Logger logger, IOptions<AppSystemType> appsystemtype, IConfiguration configuration, IOptions<AppChannel> channel)
        {
            _appSystemType = appsystemtype.Value;
            _config = configuration;
            _logger = logger;
            _appChannel = channel.Value;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DB_A57DC4_PNADb"));
            }
        }

        public async Task<ResponseParam> JsonRequestLog(object data)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            try
            {
                long unixTime = ((DateTimeOffset)logdate).ToUnixTimeSeconds();
                string JsonResult = JsonConvert.SerializeObject(data);
                string path1 = @"Resources\JsonLog\" + "RequestLog" + logdate.Year + unixTime + ".json";
                using (var tw = new StreamWriter(path1, true))
                {
                    tw.WriteLine(JsonResult.ToString());
                    tw.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[MM][jsonlog][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> JsonErrorLog(object data)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            DateTime serverTime = DateTime.Now; // gives you current Time in server timeZone
            DateTime utcTime = serverTime.ToUniversalTime(); // convert it to Utc using timezone setting of server computer
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local
            var logdate = localTime;
            try
            {
                long unixTime = ((DateTimeOffset)logdate).ToUnixTimeSeconds();
                string JsonResult = JsonConvert.SerializeObject(data);
                string path1 = @"Resources\JsonLog\" + "ErrorLog" + logdate.Year + unixTime + ".json";
                using (var tw = new StreamWriter(path1, true))
                {
                    tw.WriteLine(JsonResult.ToString());
                    tw.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[MM][jsonlog][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> sys_Log(Sys_logReq sys_LogReq)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                if (sys_LogReq.Usertype != _appSystemType.Type1 || sys_LogReq.Usertype != _appSystemType.Type2 || sys_LogReq.Usertype != _appSystemType.Type3 || sys_LogReq.Usertype != _appSystemType.Type4 || sys_LogReq.Usertype != _appSystemType.Type5 || sys_LogReq.Usertype != _appSystemType.Type6 || sys_LogReq.Usertype != _appSystemType.Type7 || sys_LogReq.Usertype != _appSystemType.Type8 || sys_LogReq.Usertype != _appSystemType.Type9 || sys_LogReq.Usertype != _appSystemType.Type10)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Not a Valid App SystemType User,Kindly contact Support Team to provide detail");
                }
                if (sys_LogReq.channel != _appChannel.Channel1 || sys_LogReq.channel != _appChannel.Channel2)
                {
                    response = new UResponseHandler().CommitResponse(requestId, ResponseCodes.ERROR, "Not a Valid App Channel ,Kindly contact Support Team to provide detail");
                }
                {
                }
                using (IDbConnection conn = Connection)
                {
                    var logdate = DateTime.UtcNow;
                    string sqlQuery = "INSERT INTO [DB_A57DC4_pnaDb_sys_logs]([Usertype],[channel],[description],[UserID],[logDate]) VALUES(@Usertype,@channel,@description,@UserID,@logdate)";
                    var results = await conn.ExecuteAsync(sqlQuery,
                         new
                         {
                             sys_LogReq.Usertype,
                             sys_LogReq.channel,
                             sys_LogReq.description,
                             sys_LogReq.UserID,
                             logdate
                         });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[MM][system ActivitLogs][Response] => {ex.Message} | [requestId]=> {requestId}");
            }
            return response;
        }
    }
}