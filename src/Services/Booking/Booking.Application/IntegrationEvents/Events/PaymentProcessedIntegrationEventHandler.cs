using Booking.Domain.AggregatesModel.BookingAggregate;
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
    public class PaymentProcessedIntegrationEventHandler : IIntegrationEventHandler<PaymentProcessedIntegrationEvent>
    {
        private readonly IBookingRespository _bookingContext;
        private TelemetryClient telemetry;

        public PaymentProcessedIntegrationEventHandler(IBookingRespository bookingContext, TelemetryClient telemetry)
        {
            _bookingContext = bookingContext;
            this.telemetry = telemetry;
        }

        public async Task Handle(PaymentProcessedIntegrationEvent eventMsg)
        {

            if (eventMsg.Id != Guid.Empty)
            {
                try
                {
                    var booking = await _bookingContext.FindByIdAsync(eventMsg.BookingOrderId);
                    
                    if (eventMsg.PaymentStatus == PaymetStatus.Completed)
                    {
                        booking.PaymentID = eventMsg.PaymentId;
                        booking.BookingState = bookingStateEnum.Completed;
                        booking.OrderStatus = "PaymentProcessed";
                    }
                    else if (eventMsg.PaymentStatus == PaymetStatus.Canceled)
                    {                       
                        booking.BookingState = bookingStateEnum.Canceled;
                    }
                    else if (eventMsg.PaymentStatus == PaymetStatus.Pending)
                    {
                        booking.BookingState = bookingStateEnum.Pending;
                    }
                                       

                    await _bookingContext.UpdateAsync(booking);
                    

                }
                catch (Exception e)
                {
                    var ExceptionTelemetry = new ExceptionTelemetry(e);
                    ExceptionTelemetry.Properties.Add("PaymentProcessedIntegrationEvent", eventMsg.BookingOrderId);
                    ExceptionTelemetry.SeverityLevel = SeverityLevel.Critical;

                    telemetry.TrackException(ExceptionTelemetry);
                   
                    throw;
                }
                
            }
        }
    }
}
