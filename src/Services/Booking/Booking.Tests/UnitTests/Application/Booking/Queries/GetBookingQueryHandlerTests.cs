using Booking.Application.Booking.Queries.DTO;
using Booking.Application.Booking.Queries.GetBooking;
using Booking.Domain.AggregatesModel.BookingAggregate;
using Booking.Domain.Booking;
using Booking.Persistence;
using Booking.Tests.UnitTests.Application.Infrastructure;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
namespace Booking.Tests.UnitTests.Application.Booking.Queries
{
    [Collection("QueryCollection")]
    public class GetBookingQueryHandlerTests
    {
        private readonly IBookingRespository _repo;       

        public GetBookingQueryHandlerTests(QueryTestFixture fixture)
        {
            _repo = fixture.repo;            
        }

        [Fact]
        public async Task GetBookingOrderAndDetails()
        {
            //Arrange
            var sut = new GetBookingQueryHandler(_repo);
            
            //Act
            GetBookingQuery req = new GetBookingQuery();
            req.BookingId = "1e4199f0-907f-4acc-b886-b12b0323c108";
            var result = await sut.Handle(req, CancellationToken.None);
                        
            //Assert

            //Main booking
            result.ShouldBeOfType<BookingOrder>();
            result.BookingOrderId.ShouldBe("1e4199f0-907f-4acc-b886-b12b0323c108");
            result.Origin.ShouldBe("NY");
            result.Destination.ShouldBe("Melbourne");
            
            //booking Details
            result.BookingDetails.Count.ShouldBe(2);

            List<BookingOrderDetail> bookingDetails = new List<BookingOrderDetail>();            
            bookingDetails.Add(new BookingOrderDetail {  PackageType = "Express" , PackageDescription = "Toys", Price = 10});
            bookingDetails.Add(new BookingOrderDetail { PackageType = "Standard", PackageDescription = "Books", Price = 50 });

            var bookingDetailsFromDB = result.BookingDetails.ToList();
            bookingDetailsFromDB[0].Price.ShouldBe(bookingDetails[0].Price);
            bookingDetailsFromDB[1].Price.ShouldBe(bookingDetails[1].Price);

            bookingDetailsFromDB[0].PackageDescription.ShouldBe(bookingDetails[0].PackageDescription);
            bookingDetailsFromDB[1].PackageDescription.ShouldBe(bookingDetails[1].PackageDescription);

            bookingDetailsFromDB[0].PackageType.ShouldBe(bookingDetails[0].PackageType);
            bookingDetailsFromDB[1].PackageType.ShouldBe(bookingDetails[1].PackageType);
        }
    }
}
