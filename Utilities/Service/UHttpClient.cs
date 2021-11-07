using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Threading.Tasks;
using Utilities.Binders;
using Utilities.Interface;

namespace Utilities.Service
{
    public class UHttpClient : UIHttpClient
    {
        private IRestResponse _restResponse;
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly BaseUrls _baseUrls;
        private readonly AppKeys _appKeys;

        public UHttpClient(IRestResponse restResponse, IOptions<BaseUrls> baseUrls, IOptions<AppKeys> appKeys)
        {
            _restResponse = restResponse;
            _baseUrls = baseUrls.Value;
            _appKeys = appKeys.Value;
        }

        public async Task<string> Post(string parameter, string url, string requestId, string header = null, string token = null)
        {
            try
            {
                RestClient client = new RestClient(url);

                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");

                request.AddParameter("application/json", parameter, ParameterType.RequestBody);

                if (url.StartsWith(_baseUrls.monnifyUrl))
                {
                    String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_appKeys.apiKey + ":" + _appKeys.clientSecret));
                    request.AddHeader("Authorization", "Basic " + encoded);
                    //request.AddParameter("Authorization", $"Bearer {_authTokens.Tokens}", ParameterType.HttpHeader);
                    //request.AddHeader("Authorization:", $"Bearer {_authTokens.Tokens}");
                }
                if (url.StartsWith(_baseUrls.monnifyUrl2))
                {
                    String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_appKeys.apiKey + ":" + _appKeys.clientSecret));
                    request.AddHeader("Authorization", "Basic " + encoded);
                    //request.AddParameter("Authorization", $"Bearer {_authTokens.Tokens}", ParameterType.HttpHeader);
                    //request.AddHeader("Authorization:", $"Bearer {_authTokens.Tokens}");
                }
                _restResponse = await client.ExecuteAsync(request);

                _logger.Info($"REQUEST MADE TO ENDPOINT =>{url};{Environment.NewLine}" +
                    $"REQUEST BODY => {parameter};{Environment.NewLine}" +
                    $"REQUEST ID => {requestId};{Environment.NewLine}" +
                    $"RESPONSE => {_restResponse.Content}" +
                    $"ErrorIfAny => {_restResponse.ErrorException}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Message => {ex.Message}; {Environment.NewLine}" +
                    $"ENDPOINT => {url};{Environment.NewLine}" +
                    $"REQUESTBODY => {parameter};{Environment.NewLine}" +
                    $"REQUEST ID => {requestId};{Environment.NewLine}" +
                    $"HTTPRequestError => {_restResponse.ErrorException}");
            }
            return _restResponse.Content;
        }

        public async Task<string> Get(string url, string requestId)
        {
            RestClient client = new RestClient($"{url}");

            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");

            if (url.StartsWith(_baseUrls.monnifyUrl))
            {
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_appKeys.apiKey + ":" + _appKeys.clientSecret));
                request.AddHeader("Authorization", "Basic " + encoded);
                //request.AddParameter("Authorization", $"Bearer {_authTokens.Tokens}", ParameterType.HttpHeader);
                //request.AddHeader("Authorization:", $"Bearer {_authTokens.Tokens}");
            }
            _restResponse = await client.ExecuteAsync(request);

            _logger.Info($"REQUEST MADE TO ENDPOINT =>{url};{Environment.NewLine}" +
                $"REQUEST Id => {requestId};{Environment.NewLine}" +
                $"RESPONSE => {_restResponse.Content}" +
                $"ErrorIfAny => {_restResponse.ErrorException}");

            return _restResponse.Content;
        }
    }
}