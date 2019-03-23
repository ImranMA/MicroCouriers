using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
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
    public class OrderPickedIntegrationEventHandler : IIntegrationEventHandler<OrderPickedIntegrationEvent>
    {
        private readonly ITrackingRepository _trackingContext;
        private static List<Type> _assemblyTypes;
        private readonly IEventBus _eventBus;
        private TelemetryClient telemetry;

        public OrderPickedIntegrationEventHandler(ITrackingRepository trackingContext, IEventBus eventBus, TelemetryClient telemetry)
        {
            _trackingContext = trackingContext;
            _assemblyTypes = TypeResolver.AssemblyTypes;
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.telemetry = telemetry;
        }

        public async Task Handle(OrderPickedIntegrationEvent eventMsg)
        {
            List<EventBase> events = new List<EventBase>();

            if (eventMsg.Id != Guid.Empty)
            {
                RequestTelemetry requestTelemetry = new RequestTelemetry { Name = "OrderPicked - Dequeue" };
                requestTelemetry.Context.Operation.Id = Guid.NewGuid().ToString();
                requestTelemetry.Context.Operation.ParentId = eventMsg.Id.ToString();

                var operation = telemetry.StartOperation(requestTelemetry);

                try
                {                   

                    Track trackings = await _trackingContext.GetTrackingAsync(eventMsg.BookingId);

                    if (trackings == null)
                        trackings = new Track();

                    var messageType = _assemblyTypes
                      .Where(t => t.Name.Contains("OrderPicked")).FirstOrDefault().
                      Name.ToString();

                    OrderPicked orderPicked = new
                        OrderPicked(eventMsg.BookingId, eventMsg.Description, eventMsg.Id
                        , messageType, eventMsg.CreationDate);


                    events.AddRange(trackings.OrderPicked(orderPicked));

                    await _trackingContext.SaveTrackingAsync(eventMsg.BookingId, trackings.OriginalVersion,
                        trackings.Version, events);

                    //Publish the event here
                    //Create Integration Event
                    var orderStatusChanged = new OrderStatusChangedIntegrationEvent(eventMsg.BookingId, "OrderPicked");
                    _eventBus.Publish(orderStatusChanged);

                    operation.Telemetry.ResponseCode = "200";
                }
                catch (Exception e)
                {
                    operation.Telemetry.ResponseCode = "500";
                    telemetry.TrackException(e);
                    throw;
                }
                finally
                {
                    // Update status code and success as appropriate.                
                    telemetry.StopOperation(operation);
                }

            }
        }
    }
}
