using MediatR;

namespace Booking.Application.Booking.Queries.GetBooking
{
    public class GetBookingQuery : IRequest<BookingOrderDTO>
    {
        public string BookingId { get; set; }
    }
}
