using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Domain.AggregatesModel.TrackingAggregate;
using Tracking.Domain.Events;

namespace Tracking.Domain.Interfaces
{
    public interface ITrackingRepository
    {
        void EnsureDatabase();
        Task<Track> GetTrackingAsync(string bookingId);
        Task SaveTrackingAsync(string bookingId, int originalVersion, int newVersion, 
            IEnumerable<EventBase> newEvents);      
    }
}
