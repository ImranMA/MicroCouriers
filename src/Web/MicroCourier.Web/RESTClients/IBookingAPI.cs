using MicroCourier.Web.Commands;
using MicroCourier.Web.DTO;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroCourier.Web.RESTClients
{
    public interface IBookingAPI
    {    
        Task<BookingOrderDTO> GetBookingById(string bookingId);

        Task<string> CreatedBooking(CreateBookingCommand command);
    }
}
