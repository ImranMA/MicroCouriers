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
    public class TrackingAPI : ITrackingAPI
    {
        private readonly HttpClient _client;
    
        public TrackingAPI(IConfiguration config, HttpClient httpclient)
        {
            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("TrackingAPI");
            string baseUri = $"http://{apiHostAndPort}";

            _client = httpclient;         
            _client.BaseAddress = new Uri(baseUri);

        }

        public async Task<TrackingDTO> GetOrderHistory(string bookingId)
        {
            try
            {
                var result = await _client.GetAsync("/api/tracking/" + bookingId);
                return await result.Content.ReadAsAsync<TrackingDTO>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
