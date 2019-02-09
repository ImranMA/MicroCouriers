using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Application.Booking.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<string>
    {
        public string BookingOrderId { get; set; }

        public string CustomerId { get; set; }
    }
}
