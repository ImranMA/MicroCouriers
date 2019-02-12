using Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.Application.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, string>
    {
        private readonly IBookingRespository _context;

        public CreateBookingCommandHandler(IBookingRespository context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var entity = new BookingOrder
            {
                BookingOrderId = Guid.NewGuid().ToString(),
                CustomerId = request.CustomerId              
            };
            return await _context.AddAsync(entity);
        }
    }
}
