using Booking.Domain.Booking;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.IntegrationEvents.Events
{
    public class OrderStatusChangedIntegrationEventHandler : IIntegrationEventHandler<OrderStatusChangedIntegrationEvent>
    {
        private readonly IBookingRespository _bookingContext;
        private TelemetryClient telemetry;

        public OrderStatusChangedIntegrationEventHandler(IBookingRespository bookingContext, TelemetryClient telemetry)
        {
            _bookingContext = bookingContext;
            this.telemetry = telemetry;
        }

        public async Task Handle(OrderStatusChangedIntegrationEvent eventMsg)
        {       

            if (eventMsg.Id != Guid.Empty)
            {
                try
                {
                    var booking = await _bookingContext.FindByIdAsync(eventMsg.BookingId);
                    booking.OrderStatus = eventMsg.CurrentStatus;

                    await _bookingContext.UpdateAsync(booking);                  
                }
                catch (Exception e)
                {               

                    var ExceptionTelemetry = new ExceptionTelemetry(e);
                    ExceptionTelemetry.Properties.Add("OrderStatusChangedIntegrationEvent", eventMsg.BookingId);
                    ExceptionTelemetry.SeverityLevel = SeverityLevel.Critical;

                    telemetry.TrackException(ExceptionTelemetry);                   
                    throw; //Throw the message so message queue abandons it
                }               

            }
        }
    }
}
