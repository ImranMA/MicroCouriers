using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Application.Booking.Commands.UpdateBooking
{
    public class UpdateBooking : IRequest<Unit>
    {
        public string BookingOrderId { get; set; }
        public string PaymentID { get; set; }
        public string NotificationID { get; set; }
    }
}
