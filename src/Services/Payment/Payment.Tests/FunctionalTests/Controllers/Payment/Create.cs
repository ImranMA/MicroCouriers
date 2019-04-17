using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using Payment.Tests.FunctionalTests.Common;
using Payment.API;
using Payment.Application.DTO;

namespace Payment.Tests.FunctionalTests.Controllers.Payment
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
            PaymentDTO payment = new PaymentDTO();
            payment.BookingOrderId = "1e4199f0-907f-4acc-b886-b12b0323c108";
            payment.Price = 100;
            payment.CustomerId = string.Empty;

            var content = Utilities.GetRequestContent(payment);
            var response = await _client.PostAsync($"/api/payment", content);

            response.EnsureSuccessStatusCode();
        }
    }
}
