using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Abstractions;
using Moq;
using Payment.Application.DTO;
using Payment.Application.Interface;
using Payment.Application.PaymentService;
using Payment.Domain.Interfaces;
using Payment.Tests.UnitTests.Application.Infrastructure;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Payment.Tests.UnitTests.Application.Payment
{
    [Collection("PaymentCollection")]

    public class PaymentServiceTest
    {
        private readonly IPaymentRepository _repo;

        public PaymentServiceTest(PaymentTestFixture fixture)
        {
            _repo = fixture.repo;
        }

        [Fact]
        public async Task AddPaymentAsync()
        {
            //Arrange
            var fakeEventBus = new Mock<IEventBus>();
            var fakeGateWay = new Mock<IPaymentExternalGateway>();

            PaymentDTO payment = new PaymentDTO();
            payment.BookingOrderId = "1e4199f0-907f-4acc-b886-b12b0323c108";
            payment.Price = 100;
            payment.CustomerId = string.Empty;                  

            var sut = new PaymentService(_repo,fakeGateWay.Object, fakeEventBus.Object);

            //Act           
            var result = await sut.AddAsync(payment);

            //Assert
            Guid bookingRef = Guid.Parse(result);
            result.ShouldBeOfType<string>();
            bookingRef.ShouldBeOfType<Guid>();
            result.Count().ShouldBe(36);
        }
    }
}
