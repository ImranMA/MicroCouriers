using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.Application.Booking.Queries.GetBooking
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingOrderDTO>
    {
        private readonly IBookingRespository _context;

        public GetBookingQueryHandler(IBookingRespository context)
        {
            _context = context;
        }

        public async Task<BookingOrderDTO> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            /*  BookingOrder bookingObj = await _context.FindByIdAsync(request.BookingId);


              var bookingDTO = new BookingOrderDTO
              {
                  BookingOrderId = bookingObj.BookingOrderId,
                  CustomerId = bookingObj.CustomerId               
              };

              ICollection<BookingOrderDetailDTO> listBoookingDetails = new List<BookingOrderDetailDTO>();
              foreach (BookingOrderDetail bookdetails in bookingObj.BookingDetails)
              {
                  var bookingDetailsObj= new BookingOrderDetailDTO
                  {
                      Price = bookdetails.Price,
                      Origin = bookdetails.Origin,
                      Destination = bookdetails.Destination,
                      PackageType = bookdetails.PackageType
                  };

                  listBoookingDetails.Add(bookingDetailsObj);
              }

              bookingDTO.BookingDetails = listBoookingDetails;

              return bookingDTO;*/
            return new BookingOrderDTO();
        }
    }
}
