using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.Notification.Email;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Notification.Interface;
using Utilities;
using Utilities.Binders;
using Utilities.SystemActivity.Interface;

namespace AppCore.Application.TransactionNotification.Service
{
    public class TransactionNotificationService : ITransactionNotification
    {
        private readonly Logger _logger;
        private readonly AppKeys _appKeys;
        private readonly IConfiguration _configuration;
        private readonly ISystemActivityService _systemActivityService;
        private readonly AppSystemType _appSystemType;
        private readonly AppChannel _appChannel;
        private readonly INotificationServices _notificationServices;
       
        public TransactionNotificationService(Logger logger, IOptions<AppKeys> appKeys, IConfiguration configuration, ISystemActivityService systemUtility, IOptions<AppSystemType> appsystemtype, IOptions<AppChannel> channel, INotificationServices notificationServices)
        {
            _appKeys = appKeys.Value;
            _logger = logger;
            _configuration = configuration;
            _systemActivityService = systemUtility;
            _appSystemType = appsystemtype.Value;
            _appChannel = channel.Value;
            _notificationServices = notificationServices;
          
        }
        public async Task<ResponseParam> CR(TransactionEmailDTO transactionEmailDTO)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                var subject = $"Credit Transaction Alert on {transactionEmailDTO.result}";
                string path = @"Resources\EmailTemplate\receipt.html";
                string fullPath;
                fullPath = Path.GetFullPath(path);
                StreamReader str = new StreamReader(fullPath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[transferamount]", transactionEmailDTO.transferamount);
                MailText = MailText.Replace("[transactiontype]", transactionEmailDTO.transactiontype);
                MailText = MailText.Replace("[getwalletid]", transactionEmailDTO.result);
                MailText = MailText.Replace("[getRwalletid]", transactionEmailDTO.result2);
                MailText = MailText.Replace("[transtype]", transactionEmailDTO.transtype);
                MailText = MailText.Replace("[fullname]", transactionEmailDTO.fullname);
                MailText = MailText.Replace("[Rfullname]", transactionEmailDTO.Rfullname);
                MailText = MailText.Replace("[narration]", transactionEmailDTO.narration);
                MailText = MailText.Replace("[transactionref]", transactionEmailDTO.transactionref);
                MailText = MailText.Replace("[transactiondate]", transactionEmailDTO.transactiondate);
                var SendEmailApi = new SendEmailApiReq()
                {
                    htmlBody = MailText,
                    MailSubject = subject,
                    MailTo = transactionEmailDTO.REmail
                };

                await _notificationServices.sendmail(SendEmailApi);
            }
            catch (Exception ex)
            {

                _logger.LogError($"[PNAAPI][Transaction Email][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemActivityService.JsonErrorLog($"[PNAAPI][Transaction Email][Response] => {ex.Message} | [request]=> " +
                                                JsonConvert.SerializeObject(transactionEmailDTO) + $"| [requestId]=> {requestId}");
            }
            return response;
        }

        public async Task<ResponseParam> DR(TransactionEmailDTO transactionEmailDTO)
        {
            string requestId = string.Empty;
            requestId = Helper.GenerateUniqueId(7);
            var response = UResponseHandler.InitializeResponse(requestId);
            try
            {
                var subject = $"Debit Transaction Alert on {transactionEmailDTO.result}";
                string path = @"Resources\EmailTemplate\receipt.html";
                string fullPath;
                fullPath = Path.GetFullPath(path);
                StreamReader str = new StreamReader(fullPath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[transferamount]", transactionEmailDTO.transferamount);
                MailText = MailText.Replace("[transactiontype]", transactionEmailDTO.transactiontype);
                MailText = MailText.Replace("[getwalletid]", transactionEmailDTO.result);
                MailText = MailText.Replace("[getRwalletid]", transactionEmailDTO.result2);
                MailText = MailText.Replace("[transtype]", transactionEmailDTO.transtype);
                MailText = MailText.Replace("[fullname]", transactionEmailDTO.fullname);
                MailText = MailText.Replace("[Rfullname]", transactionEmailDTO.Rfullname);
                MailText = MailText.Replace("[narration]", transactionEmailDTO.narration);
                MailText = MailText.Replace("[transactionref]", transactionEmailDTO.transactionref);
                MailText = MailText.Replace("[transactiondate]", transactionEmailDTO.transactiondate);
                var SendEmailApis = new SendEmailApiReq()
                {
                    htmlBody = MailText,
                    MailSubject = subject,
                    MailTo = transactionEmailDTO.SEmail
                };

                await _notificationServices.sendmail(SendEmailApis);
            }
            catch (Exception ex)
            {

                _logger.LogError($"[PNAAPI][Transaction Email][Response] => {ex.Message} | [requestId]=> {requestId}");
                await _systemActivityService.JsonErrorLog($"[PNAAPI][Transaction Email][Response] => {ex.Message} | [request]=> " +
                                               JsonConvert.SerializeObject(transactionEmailDTO) +"| [requestId]=> {requestId}");

            }
            return response;
        }
    }
}
