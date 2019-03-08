using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Application.IntegrationEvents.Events
{
    public class OrderStatusChangedIntegrationEvent : IntegrationEvent
    {
        public string BookingId { get; set; }
        public string CurrentStatus { get; set; }
        public OrderStatusChangedIntegrationEvent(string bookingId, string currentStatus)
        {
            CurrentStatus = currentStatus;
            BookingId = bookingId;
        }
    }
}
