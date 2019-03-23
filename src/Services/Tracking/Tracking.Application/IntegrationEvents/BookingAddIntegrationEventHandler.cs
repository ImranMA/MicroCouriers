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
    public class BookingAddIntegrationEventHandler : IIntegrationEventHandler<BookingAddIntegrationEvent>
    {
        private readonly ITrackingRepository _trackingContext;
        private static List<Type> _assemblyTypes;
        private TelemetryClient telemetry;

        public BookingAddIntegrationEventHandler(ITrackingRepository trackingContext, TelemetryClient telemetry)
        {
            _trackingContext = trackingContext;
            _assemblyTypes = TypeResolver.AssemblyTypes;
            this.telemetry = telemetry;
        }

        public async Task Handle(BookingAddIntegrationEvent eventMsg)
        {
            List<EventBase> events = new List<EventBase>();

            if (eventMsg.Id != Guid.Empty)
            {
                RequestTelemetry requestTelemetry = new RequestTelemetry { Name = "BookingCreated - Dequeue" };
                requestTelemetry.Context.Operation.Id = Guid.NewGuid().ToString();
                requestTelemetry.Context.Operation.ParentId = eventMsg.Id.ToString();
              

                var operation = telemetry.StartOperation(requestTelemetry);

                try
                {
                    Track trackings = await _trackingContext.GetTrackingAsync(eventMsg.BookingId);

                    if (trackings == null)
                        trackings = new Track();
                   
                    var messageType = _assemblyTypes
                      .Where(t => t.Name.Contains("BookingCreated")).FirstOrDefault().
                      Name.ToString();

                    BookingCreated bookingCreated = new
                        BookingCreated(eventMsg.BookingId, string.Empty,eventMsg.Id
                        ,messageType,eventMsg.CreationDate,eventMsg.Origin,eventMsg.Destination);


                    events.AddRange(trackings.BookingAdd(bookingCreated));

                    await _trackingContext.SaveTrackingAsync(eventMsg.BookingId, trackings.OriginalVersion,
                        trackings.Version, events);

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
