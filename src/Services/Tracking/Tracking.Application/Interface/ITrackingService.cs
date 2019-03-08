using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tracking.Application.DTO;

namespace Tracking.Application.Interface
{
    public interface ITrackingService
    {
        Task<TrackingDTO> FindByIdAsync(string bookingId);
    }
}
