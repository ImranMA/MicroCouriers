using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroCourier.Web.Commands
{
    public class CreateBookingCommand 
    {
        public readonly ICollection<BookingOrderDetails> BookingDetails;

        public CreateBookingCommand(string bookingOrderId, string customerId)
        {
            CustomerId = customerId;
            BookingDetails = new List<BookingOrderDetails>();
        }

        public string Origin { get; set; }
        public string Destination { get; set; }
        public string CustomerId { get; set; }

    }

    public class BookingOrderDetails
    {
        public string PackageType { get; set; }
        public string PackageDescription { get; set; }
        public decimal Price { get; set; }
    }
}
