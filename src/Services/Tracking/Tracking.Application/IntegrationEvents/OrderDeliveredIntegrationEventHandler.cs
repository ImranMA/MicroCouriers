using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracking.Common;
using Tracking.Domain.AggregatesModel.TrackingAggregate;
using Tracking.Domain.Events;
using Tracking.Domain.Interfaces;
namespace Tracking.Application.IntegrationEvents
{
    public class OrderDeliveredIntegrationEventHandler : IIntegrationEventHandler<OrderDeliveredIntegrationEvent>
    {
        private readonly ITrackingRepository _trackingContext;
        private static List<Type> _assemblyTypes;

        public OrderDeliveredIntegrationEventHandler(ITrackingRepository trackingContext)
        {
            _trackingContext = trackingContext;
            _assemblyTypes = TypeResolver.AssemblyTypes;
        }

        public async Task Handle(OrderDeliveredIntegrationEvent eventMsg)
        {
            List<EventBase> events = new List<EventBase>();

            if (eventMsg.Id != Guid.Empty)
            {
                try
                {
                    Track trackings = await _trackingContext.GetTrackingAsync(eventMsg.BookingId);

                    if (trackings == null)
                        trackings = new Track();

                    var messageType = _assemblyTypes
                      .Where(t => t.Name.Contains("OrderDelivered")).FirstOrDefault().
                      Name.ToString();

                    OrderDelivered orderDelivered = new
                        OrderDelivered(eventMsg.BookingId, eventMsg.Description, eventMsg.Id
                        , messageType, eventMsg.CreationDate,eventMsg.SignedBy);


                    events.AddRange(trackings.OrderDelivered(orderDelivered));

                    await _trackingContext.SaveTrackingAsync(eventMsg.BookingId, trackings.OriginalVersion,
                        trackings.Version, events);
                }
                catch (Exception ex)
                {

                }


            }
        }
    }
}
