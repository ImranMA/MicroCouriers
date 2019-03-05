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

    public class OrderTransitIntegrationEventHandler : IIntegrationEventHandler<OrderTransitIntegrationEvent>
    {
        private readonly ITrackingRepository _trackingContext;
        private static List<Type> _assemblyTypes;

        public OrderTransitIntegrationEventHandler(ITrackingRepository trackingContext)
        {
            _trackingContext = trackingContext;
            _assemblyTypes = TypeResolver.AssemblyTypes;
        }

        public async Task Handle(OrderTransitIntegrationEvent eventMsg)
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
                      .Where(t => t.Name.Contains("OrderInTransit")).FirstOrDefault().
                      Name.ToString();

                    OrderInTransit orderInTransit = new
                        OrderInTransit(eventMsg.BookingId, eventMsg.Description, eventMsg.Id
                        , messageType, eventMsg.CreationDate);


                    events.AddRange(trackings.OrderInTransit(orderInTransit));

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
