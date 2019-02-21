using Booking.Domain.AggregatesModel.BookingAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Booking
{
    public interface IBookingRespository
    {
        Task<string> AddAsync(BookingOrder bookingOrder);
        Task<BookingOrder> UpdateAsync(BookingOrder bookingOrder);     
        Task<BookingOrder> FindByIdAsync(string bookingOrderId);
    }
}
