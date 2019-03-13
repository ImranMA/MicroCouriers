using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderShipping.Events
{
    public class OrderDeliveredIntegrationEvent : IntegrationEvent
    {
        public string BookingId { get; set; }
        public string Description { get; set; }
        public string SignedBy { get; set; }

        public OrderDeliveredIntegrationEvent(string bookingId, string description, string signedBy)
        {
            BookingId = bookingId;
            Description = description;
            SignedBy = signedBy;
        }
    }
}
