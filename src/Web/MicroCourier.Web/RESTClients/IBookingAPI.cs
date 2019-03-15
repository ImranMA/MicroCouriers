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
        [Get("/booking/{id}")]
        Task<BookingOrderDTO> GetBookingById([AliasAs("id")] string bookingId);

        [Post("/booking")]
        Task<string> CreatedBooking(CreateBookingCommand command);
    }
}
