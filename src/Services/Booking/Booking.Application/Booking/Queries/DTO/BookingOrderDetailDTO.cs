using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Application.Booking.Queries.DTO
{
    public class BookingOrderDetailDTO
    {
        public string PackageType { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Price { get; set; }
    }
}
