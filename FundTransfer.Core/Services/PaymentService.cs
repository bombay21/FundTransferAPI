using FundTransfer.Core.DTOs;
using FundTransfer.Core.Entities;
using FundTransfer.Core.Interfaces;
using FundTransfer.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace FundTransfer.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _config;
        private string key;

        public PaymentService(IConfiguration _configuration)
        {
            _config = _configuration;
            key = _config.GetSection("ApiSettings")["PaystackSK"];
        }

        public async Task<K> PostRequest<K, T>(T model, string url)
        {
            try
            {
                string serialized = JsonConvert.SerializeObject(model);
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", $"bearer {key}");
                request.AddParameter("application/json", serialized, ParameterType.RequestBody);
                IRestResponse response = await client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<K>(response.Content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> GetRequest<T>(string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", $"bearer {key}");

                IRestResponse<T> response = await client.ExecuteAsync<T>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
