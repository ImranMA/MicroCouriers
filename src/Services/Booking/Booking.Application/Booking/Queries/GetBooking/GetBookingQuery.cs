using Booking.Domain.AggregatesModel.BookingAggregate;
using MediatR;

namespace Booking.Application.Booking.Queries.GetBooking
{
    public class GetBookingQuery : IRequest<BookingOrder>
    {
        public string BookingId { get; set; }
    }
}
