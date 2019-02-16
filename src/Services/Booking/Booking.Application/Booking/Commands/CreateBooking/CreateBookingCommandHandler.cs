using Booking.Domain.AggregatesModel.BookingAggregate;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.Application.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, string>
    {
        private readonly IBookingRespository _bookingContext;

        public CreateBookingCommandHandler(IBookingRespository context)
        {
            _bookingContext = context;
        }

        public async Task<string> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var bookingOrder = new BookingOrder(request.CustomerId);
                        
            foreach (BookingOrderDetails bookdetails in request.BookingDetails)
            {
                bookingOrder.AddBookingDetails(bookingOrder.BookingOrderId,
                    bookdetails.PackageType, bookdetails.Origin, bookdetails.Destination, bookdetails.Price);
            }

            return await _bookingContext.AddAsync(bookingOrder);
        }
    }
}
