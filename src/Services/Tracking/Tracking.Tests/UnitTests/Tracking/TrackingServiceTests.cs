using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracking.Application.DTO;
using Tracking.Application.TrackingServices;
using Tracking.Common;
using Tracking.Domain.AggregatesModel.TrackingAggregate;
using Tracking.Domain.Events;
using Tracking.Domain.Interfaces;
using Xunit;

namespace Tracking.Tests.UnitTests.Tracking
{
    public class TrackingServiceTests
    {
        private JsonSerializerSettings _serializerSettings;
        public TrackingServiceTests()
        {
            _serializerSettings = new JsonSerializerSettings();
            _serializerSettings.Formatting = Formatting.Indented;
            _serializerSettings.Converters.Add(new StringEnumConverter
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            });
        }

        [Fact]
        public async Task GetTracking()
        {
            //Arrange
            var fakeRepo = new Mock<ITrackingRepository>();
            List<EventBase> events = new List<EventBase>();

            string bookingId = "1e4199f0-907f-4acc-b886-b12b0323c108";

            PaymentProcessed paymentProc = new PaymentProcessed(bookingId, "Payment is Completed",
                Guid.NewGuid(), "PaymentProcessed", DateTime.Now);

            OrderPicked orderPicked = new OrderPicked(bookingId, "Order is picked from Origin", Guid.NewGuid(),
             "OrderPicked", DateTime.Now);

            events.Add(paymentProc);
            events.Add(orderPicked);

            Track tracking = new Track(events);

            fakeRepo.Setup(m => m.GetTrackingAsync(It.IsAny<string>())).Returns(Task.FromResult(tracking));
            var sut = new TrackingSerivceWithoutCache(fakeRepo.Object);

            //Act           
            var result = await sut.FindByIdAsync(bookingId);
            var jArray = JArray.Parse(result.TrackingHistory);          
            var item = JObject.FromObject(jArray[0]);


            //Assert            
            result.BookingId.ShouldBe(bookingId);
            result.ShouldBeOfType<TrackingDTO>();
            item.GetValue("Description").ShouldBe("Payment is Completed");
            item.GetValue("BookingOrderId").ShouldBe(bookingId);

        }
        
    }
}
