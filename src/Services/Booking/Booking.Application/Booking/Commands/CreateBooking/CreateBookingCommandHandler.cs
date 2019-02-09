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


        public async Task<string> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {       

            return string.Empty;
        }
    }
}
