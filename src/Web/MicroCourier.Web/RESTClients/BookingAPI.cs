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
using Microsoft.ApplicationInsights;

namespace MicroCourier.Web.RESTClients
{
    public class BookingAPI : IBookingAPI
    {
       
        private readonly HttpClient _client;
        //private IBookingAPI _clientBooking;


        public BookingAPI(IConfiguration config, HttpClient httpclient)
        {
     
            
            _client = httpclient;
            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("BookingAPI");                     
           
            string baseUri = $"http://{apiHostAndPort}";            
            _client.BaseAddress = new Uri(baseUri);

        }

        public async Task<BookingOrderDTO> GetBookingById([AliasAs("id")] string bookingId)
        {
            try
            {
                var result = await _client.GetAsync("/api/booking/" + bookingId);

                if (result.StatusCode != HttpStatusCode.OK)
                    return null;

                return await result.Content.ReadAsAsync<BookingOrderDTO>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CreatedBooking(CreateBookingCommand command)
        {
            try
            {
                var result = await _client.PostAsync("/api/booking", new StringContent(JsonConvert.SerializeObject(command),
                     Encoding.UTF8, "application/json"));

                if (result.StatusCode != HttpStatusCode.OK)
                    return null;

                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

    }
}
