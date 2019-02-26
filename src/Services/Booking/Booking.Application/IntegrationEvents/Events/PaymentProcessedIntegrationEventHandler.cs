using Booking.Domain.AggregatesModel.BookingAggregate;
using Booking.Domain.Booking;
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

        public PaymentProcessedIntegrationEventHandler(IBookingRespository bookingContext)
        {
            _bookingContext = bookingContext;
        }

        public async Task Handle(PaymentProcessedIntegrationEvent eventMsg)
        {          

            if (eventMsg.Id != Guid.Empty)
            {
                var booking = await _bookingContext.FindByIdAsync(eventMsg.BookingOrderId);

                if(eventMsg.PaymentStatus == PaymetStatus.Completed)
                {
                    booking.PaymentID = eventMsg.PaymentId;
                    booking.BookingState = bookingStateEnum.Completed;
                }               

                await _bookingContext.UpdateAsync(booking);
            }
        }
    }
}
