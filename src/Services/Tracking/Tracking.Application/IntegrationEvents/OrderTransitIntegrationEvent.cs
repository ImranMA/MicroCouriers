using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Application.IntegrationEvents
{
    public class OrderTransitIntegrationEvent : IntegrationEvent
    {
        public string BookingId { get; set; }
        public string Description { get; set; }


        public OrderTransitIntegrationEvent(string bookingId, string description)
        {
            BookingId = bookingId;
            Description = description;
        }
    }
}
