using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Application.Booking.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<string>
    {
        private readonly ICollection<BookingOrderDetails> BookingDetails;

        public CreateBookingCommand(string bookingOrderId,string customerId)
        {
            BookingOrderId = bookingOrderId;
            CustomerId = customerId;
            BookingDetails = new List<BookingOrderDetails>();
        }
        public string BookingOrderId { get; set; }

        public string CustomerId { get; set; }
        
    }

    public class BookingOrderDetails
    {
        public string PackageType { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Price { get; set; }
    }
}
