using MicroCourier.Web.Commands;
using MicroCourier.Web.DTO;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace MicroCourier.Web.RESTClients
{
    public class PaymentAPI : IPaymentAPI
    {
        private readonly HttpClient _client;

        public PaymentAPI(IConfiguration config, HttpClient httpclient)
        {
            _client = httpclient;
            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("PaymentAPI");
            string baseUri = $"http://{apiHostAndPort}";
            _client.BaseAddress = new Uri(baseUri);

        }
        public async Task<string> CreatedPayment(PaymentDTO payment)
        {
            try
            {
                var result = await _client.PostAsync("/api/payment", new StringContent(JsonConvert.SerializeObject(payment),
               Encoding.UTF8, "application/json"));

                if (result.StatusCode != HttpStatusCode.Created)
                    throw new Exception(result.ReasonPhrase);

                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
