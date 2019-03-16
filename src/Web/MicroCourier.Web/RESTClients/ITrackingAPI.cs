using MicroCourier.Web.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroCourier.Web.RESTClients
{
    public interface ITrackingAPI
    {       
        Task<TrackingDTO> GetOrderHistory(string bookingId);
    }
}
