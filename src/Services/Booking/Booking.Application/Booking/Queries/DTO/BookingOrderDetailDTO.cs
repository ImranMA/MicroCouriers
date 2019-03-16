using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Application.Booking.Queries.DTO
{
    public class BookingOrderDetailDTO
    {
        public string PackageType { get; set; }
        public string Description { get; set; }      
        public decimal Price { get; set; }
    }
}
