using System.Threading.Tasks;
using Booking.Domain.Booking;
using Booking.Domain.AggregatesModel.BookingAggregate;

namespace Booking.Persistence.Repositories
{
    public class BookingRepository : IBookingRespository
    {
        private readonly BookingDbContext _context;

        public BookingRepository(BookingDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<string> AddAsync(BookingOrder bookingOrder)
        {
            _context.Set<BookingOrder>().Add(bookingOrder);
            await _context.SaveChangesAsync();

            return bookingOrder.BookingOrderId;
        }

        public async Task<BookingOrder> UpdateAsync(BookingOrder bookingOrder)
        {            
            _context.Bookings.Update(bookingOrder);
            await _context.SaveChangesAsync();

            return bookingOrder;
        }

        public async Task<BookingOrder> FindByIdAsync(string bookingOrderId)
        {
            var bookingOrder = await _context.Bookings.FindAsync(bookingOrderId);

            if (bookingOrder != null)
            {
                await _context.Entry(bookingOrder)
                   .Collection(i => i.BookingDetails).LoadAsync();
            }

            return bookingOrder;
        }

       
    }
}
