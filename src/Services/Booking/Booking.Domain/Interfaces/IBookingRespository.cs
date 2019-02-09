using Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Booking
{
    public interface IBookingRespository
    {
        Task<BookingOrder> AddAsync(BookingOrder bookingOrder);
        Task<BookingOrder> UpdateAsync(BookingOrder bookingOrder);     
        Task<BookingOrder> FindByIdAsync(string bookingOrderId);
    }
}
