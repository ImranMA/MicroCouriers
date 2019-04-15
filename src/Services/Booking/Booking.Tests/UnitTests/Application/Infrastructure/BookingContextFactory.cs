using Booking.Application.Booking.Queries.GetBooking;
using Booking.Domain.AggregatesModel.BookingAggregate;
using Booking.Persistence;
using Booking.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace Booking.Tests.UnitTests.Application.Infrastructure
{
    public class BookingContextFactory
    {
        public static BookingRepository Create(BookingDbContext context)
        {            
            context.Database.EnsureCreated();

            var bookingRepo = new BookingRepository(context);

            BookingOrder bookingOrder =
                new BookingOrder
                {
                    BookingOrderId = "1e4199f0-907f-4acc-b886-b12b0323c108",
                    Origin = "NY",
                    Destination = "Melbourne"
                };

            bookingOrder.AddBookingDetails("1e4199f0-907f-4acc-b886-b12b0323c108",
                   "Express", "Toys", 10);
            bookingOrder.AddBookingDetails("1b4199f0-907f-4acc-b886-b12b0323c108",
                 "Standard", "Books", 50);

            context.Bookings.AddRange(new[] { bookingOrder, bookingOrder });
            context.SaveChanges();

            return bookingRepo;
        }

        public static void Destroy(BookingDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
