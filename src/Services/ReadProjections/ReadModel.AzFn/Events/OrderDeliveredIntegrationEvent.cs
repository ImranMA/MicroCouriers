using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;

namespace ReadModel.AzFn.Events
{
    public class OrderDeliveredIntegrationEvent : IntegrationEvent
    {
        public string BookingId { get; set; }
        public string Description { get; set; }
        public string SignedBy { get; set; }

        public OrderDeliveredIntegrationEvent(string bookingId, string description ,string signedBy)
        {
            BookingId = bookingId;
            Description = description;
            SignedBy = signedBy;
        }
    }
}
