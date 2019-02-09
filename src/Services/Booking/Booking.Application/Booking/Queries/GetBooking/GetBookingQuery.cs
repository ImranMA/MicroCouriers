using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Application.Booking.Queries.GetBooking
{
    public class GetBookingQuery : IRequest<string>
    {
        public string BookingId { get; set; }
    }
}
