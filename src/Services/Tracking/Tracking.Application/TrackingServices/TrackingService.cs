using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using Tracking.Application.DTO;
using Tracking.Application.Interface;
using Tracking.Domain.Interfaces;

namespace Tracking.Application.TrackingServices
{
    public class TrackingService : ITrackingService
    {
        private readonly ITrackingRepository _context;
        private readonly IDatabase _cache;


        public TrackingService(ITrackingRepository context,
            IConnectionMultiplexer multiplexer)
        {          
            _context = context;            
            _cache = multiplexer.GetDatabase();
        }

        public async Task<TrackingDTO> FindByIdAsync(string bookingId)
        {
            //check in the redis cahce
            var bookingHistroy = await GetFromCache(bookingId);

            //if not found in the cahce
            if (string.IsNullOrEmpty(bookingHistroy))
            {
                //get from database
                var result = await _context.GetTrackingAsync(bookingId);
                if (result != null)
                {
                    //format result
                    bookingHistroy = JsonConvert.SerializeObject(result.orderHistory);
                    SetCache(bookingId, bookingHistroy);
                }               
            }

            return new TrackingDTO { BookingId = bookingId,
                TrackingHistory = bookingHistroy };
        }

        //
        private async Task<string> GetFromCache(string bookingId)
        {
            return await _cache.StringGetAsync(bookingId);
        }

        private async void SetCache(string bookingId,string bookingHistroy)
        {
            //Update Cache
            var ts = TimeSpan.FromDays(1);
            await _cache.StringSetAsync(bookingId, bookingHistroy, ts);
        }
    }
}
