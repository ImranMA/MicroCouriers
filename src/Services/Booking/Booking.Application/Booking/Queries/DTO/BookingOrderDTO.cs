using Booking.Application.Booking.Queries.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Application.Booking.Queries.GetBooking
{
    public class BookingOrderDTO
    {
        public string BookingOrderId { get; set; }
        public string CustomerId { get; set; }     

        public ICollection<BookingOrderDetailDTO> BookingDetails { get;  set; }
    }

}
