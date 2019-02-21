using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Booking.Domain.AggregatesModel.BookingAggregate;
using Booking.Domain.Booking;
using MediatR;

namespace Booking.Application.Booking.Commands.UpdateBooking
{
    public class UpdateBookingHandler : IRequestHandler<UpdateBooking, Unit>
    {
        private readonly IBookingRespository _bookingContext;

        public UpdateBookingHandler(IBookingRespository context)
        {
            _bookingContext = context;
        }

        public async Task<Unit> Handle(UpdateBooking request, CancellationToken cancellationToken)
        {
            var bookingOrder = await _bookingContext.FindByIdAsync(request.BookingOrderId);

            if (bookingOrder == null)
            {
                return Unit.Value;
            }

            bookingOrder.SetPayment(request.PaymentID);
            await _bookingContext.UpdateAsync(bookingOrder);

            return Unit.Value;
        }
    }
}
