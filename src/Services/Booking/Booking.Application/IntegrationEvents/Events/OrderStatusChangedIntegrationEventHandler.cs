using Booking.Domain.Booking;
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

        public OrderStatusChangedIntegrationEventHandler(IBookingRespository bookingContext)
        {
            _bookingContext = bookingContext;
        }

        public async Task Handle(OrderStatusChangedIntegrationEvent eventMsg)
        {

            if (eventMsg.Id != Guid.Empty)
            {
                var booking = await _bookingContext.FindByIdAsync(eventMsg.BookingId);
                booking.OrderStatus = eventMsg.CurrentStatus;

                await _bookingContext.UpdateAsync(booking);
            }
        }
    }
}
