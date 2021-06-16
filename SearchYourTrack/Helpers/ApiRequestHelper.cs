using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SearchYourTrack.Helpers
{
    public class ApiRequestHelper
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public ApiRequestHelper(ILogger logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IRestResponse GetREST(string apiEndPoint)
        {
            _logger.LogInformation("Method > GetREST > Param > apiEndPoint > " + apiEndPoint);
            var client = new RestClient(Convert.ToString(_configuration["BaseApiAddress"]) + apiEndPoint);
            var request = new RestRequest(Method.GET);
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                IRestResponse response = client.Execute(request);
                httpResponse.StatusCode = System.Net.HttpStatusCode.OK;
                return response;
            }catch(Exception ex)
            {
                _logger.LogError("Method > GetREST > Param > apiEndPoint > " + apiEndPoint + " > Error Message > " + ex.Message);
                throw;
            }
        }
    }
}
