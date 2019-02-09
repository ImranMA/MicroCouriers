using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.Application.Booking.Queries.GetBooking
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, string>
    {
        private readonly IBookingRespository _context;

        public GetBookingQueryHandler(IBookingRespository context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            return string.Empty;
        }
    }
}
