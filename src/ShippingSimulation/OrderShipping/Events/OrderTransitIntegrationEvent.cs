using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderShipping.Events
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
