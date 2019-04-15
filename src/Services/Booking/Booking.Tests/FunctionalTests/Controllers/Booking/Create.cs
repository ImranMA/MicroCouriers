using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using Booking.Tests.FunctionalTests.Common;
using Booking.Application.Booking.Commands.CreateBooking;
using Booking.API;
using System.Collections.Generic;

namespace Booking.Tests.FunctionalTests.Controllers.Booking
{
    public class Create : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public Create(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GivenCreateBookingCommand_ReturnsSuccessStatusCode()
        {           
            var command = new CreateBookingCommand(string.Empty, string.Empty);
            command.CustomerId = string.Empty;
            command.Destination = "45 Terrian st";
            command.Origin = "67 bloom blvd";

            var content = Utilities.GetRequestContent(command);
            var response = await _client.PostAsync($"/api/booking", content);

            response.EnsureSuccessStatusCode();
        }
    }
}
