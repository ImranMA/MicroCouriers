using Booking.Domain.Booking;
using Booking.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Booking.Tests.UnitTests.Application.Infrastructure
{
    public class CommandTestBase : IDisposable
    {
        public BookingDbContext Context { get; private set; }
        public IBookingRespository repo { get; private set; }

        public CommandTestBase()
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

    [CollectionDefinition("CommandCollection")]
    public class CommandCollection : ICollectionFixture<CommandTestBase> { }
}
