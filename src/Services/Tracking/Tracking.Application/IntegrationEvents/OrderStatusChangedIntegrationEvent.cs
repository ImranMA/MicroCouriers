using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Application.IntegrationEvents
{
    public  class OrderStatusChangedIntegrationEvent : IntegrationEvent
    {

        public string BookingId { get; set; }
        public string CurrentStatus { get; set; }
        public OrderStatusChangedIntegrationEvent(string bookingId ,string currentStatus)
        {
            CurrentStatus = currentStatus;
            BookingId = bookingId;
        }
        
    }
}
