using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
namespace ReadModel.AzFn.Events
{
    public class OrderPickedIntegrationEvent : IntegrationEvent
    {
        public string BookingId { get; set; }      
        public string Description { get; set; }


        public OrderPickedIntegrationEvent(string bookingId, string description)
        {
            BookingId = bookingId;
            Description = description;           
        }
    }
}
