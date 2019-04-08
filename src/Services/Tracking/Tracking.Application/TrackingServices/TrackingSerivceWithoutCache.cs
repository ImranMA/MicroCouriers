using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Tracking.Application.DTO;
using Tracking.Application.Interface;
using Tracking.Domain.Interfaces;

namespace Tracking.Application.TrackingServices
{
    public class TrackingSerivceWithoutCache : ITrackingService
    {

        private readonly ITrackingRepository _context;
       
        public TrackingSerivceWithoutCache(ITrackingRepository context)
        {
            _context = context;            
        }

        public async Task<TrackingDTO> FindByIdAsync(string bookingId)
        {
            var result = await _context.GetTrackingAsync(bookingId);
            var bookingHistroy = string.Empty;

            if (result != null)
            {
                //format result
                bookingHistroy = JsonConvert.SerializeObject(result.orderHistory);              
            }

            return new TrackingDTO
            {
                BookingId = bookingId,
                TrackingHistory = bookingHistroy
            };
        }
    }
}
