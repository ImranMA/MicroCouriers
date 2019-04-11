using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;

namespace ReadModel.AzFn.Events
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
