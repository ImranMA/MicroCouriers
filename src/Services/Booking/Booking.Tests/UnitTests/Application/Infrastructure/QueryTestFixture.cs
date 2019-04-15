using Booking.Domain.Booking;
using Booking.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Booking.Tests.UnitTests.Application.Infrastructure
{
    public class QueryTestFixture : IDisposable
    {
        public BookingDbContext Context { get; private set; }
        public IBookingRespository repo { get; private set; }

        public QueryTestFixture()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;

            Context = new BookingDbContext(options);
            repo = BookingContextFactory.Create(Context);          
        }

        public void Dispose()
        {
            BookingContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
