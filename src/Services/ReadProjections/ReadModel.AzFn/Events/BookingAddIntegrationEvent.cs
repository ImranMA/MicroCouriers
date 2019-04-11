using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;

namespace ReadModel.AzFn.Events
{
    public class BookingAddIntegrationEvent : IntegrationEvent
    {
        public string BookingId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }


        public BookingAddIntegrationEvent(string bookingId, string origin, string destination)
        {
            BookingId = bookingId;
            Origin = origin;
            Destination = destination;
        }
    }
}
