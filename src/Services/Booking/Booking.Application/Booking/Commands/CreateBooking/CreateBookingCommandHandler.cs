using MediatR;
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
         /*  var entity = new BookingOrder(Guid.NewGuid().ToString(), new BookingOrderDetails())
            {
                BookingOrderId = Guid.NewGuid().ToString(),
                CustomerId = request.CustomerId              
            };


            foreach (BookingOrderDetails bookdetails in request.BookingDetails)
            {
                var bookingDetailsObj = new BookingOrderDetail
                {
                    Price = bookdetails.Price,
                    Origin = bookdetails.Origin,
                    Destination = bookdetails.Destination,
                    PackageType = bookdetails.PackageType
                };

                entity.BookingDetails.Add(bookingDetailsObj);
            }


            return await _context.AddAsync(entity);*/

            return "1";
        }
    }
}
