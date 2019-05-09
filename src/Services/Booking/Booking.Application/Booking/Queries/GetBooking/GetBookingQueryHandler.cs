using Booking.Application.Booking.Queries.DTO;
using Booking.Domain.AggregatesModel.BookingAggregate;
using Booking.Domain.Booking;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.Application.Booking.Queries.GetBooking
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingOrder>
    {
        private readonly IBookingRespository _context;

        public GetBookingQueryHandler(IBookingRespository context)
        {
            _context = context;
        }

        public async Task<BookingOrder> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            return await _context.FindByIdAsync(request.BookingId);            
        }
    }
}
