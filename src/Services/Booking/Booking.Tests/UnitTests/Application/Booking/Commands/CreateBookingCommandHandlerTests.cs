using Booking.Application.Booking.Commands.CreateBooking;
using Booking.Application.Booking.Queries.DTO;
using Booking.Application.Booking.Queries.GetBooking;
using Booking.Domain.Booking;
using Booking.Persistence;
using Booking.Tests.UnitTests.Application.Infrastructure;
using MediatR;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Abstractions;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Booking.Tests.UnitTests.Application.Booking.Commands
{
    [Collection("CommandCollection")]

    public class CreateBookingCommandHandlerTests
    {
        private readonly IBookingRespository _repo;     

        public CreateBookingCommandHandlerTests(CommandTestBase fixture)
        {
            _repo = fixture.repo;
        }
        
        [Fact]
        public async Task CreateBooking()
        {
            //Arrange
            var _fakeEventBus = new Mock<IEventBus>();           
            var sut = new CreateBookingCommandHandler(_repo, _fakeEventBus.Object);
                       
            //Act
            CreateBookingCommand command = new CreateBookingCommand(string.Empty, string.Empty);
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Guid bookingRef = Guid.Parse(result);
            result.ShouldBeOfType<string>();
            bookingRef.ShouldBeOfType<Guid>();
            result.Count().ShouldBe(36);         
        }
    }
}
