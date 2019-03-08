using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
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
            var bookingHistroy = await _cache.StringGetAsync(bookingId);

            //if not found in the cahce
            if (string.IsNullOrEmpty(bookingHistroy))
            {
                //get from database
                var result = await _context.GetTrackingAsync(bookingId);
                if (result != null)
                {
                    //format result
                    bookingHistroy = String.Join(", ", result.orderHistory.ToArray());
                                       
                    //Update Cache
                    var ts = TimeSpan.FromDays(1);
                    await _cache.StringSetAsync(bookingId, bookingHistroy, ts);
                }               
            }

            return new TrackingDTO { BookingId = bookingId,
                TrackingHistory = bookingHistroy };
        }
    }
}
