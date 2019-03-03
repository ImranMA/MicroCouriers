using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tracking.Domain.AggregatesModel.TrackingAggregate;
using Tracking.Domain.Events;
using Tracking.Domain.Interfaces;

namespace Tracking.Application.IntegrationEvents
{
    public class PaymentProcessedIntegrationEventHandler : IIntegrationEventHandler<PaymentProcessedIntegrationEvent>
    {
        private readonly ITrackingRepository _trackingContext;

        public PaymentProcessedIntegrationEventHandler(ITrackingRepository trackingContext)
        {
            _trackingContext = trackingContext;
        }

        public async Task Handle(PaymentProcessedIntegrationEvent eventMsg)
        {
            List<EventBase> events = new List<EventBase>();

            if (eventMsg.Id != Guid.Empty)
            {
                try
                {
                    Track trackings = await _trackingContext.GetTrackingAsync(eventMsg.BookingOrderId);

                    if (trackings == null)
                        trackings = new Track();

                    string description = string.Empty;
                    if (eventMsg.PaymentStatus == PaymetStatus.Completed)
                    {
                        description = "Payment Done";
                    }

                    PaymentProcessed eventPaymentProcessed = new PaymentProcessed(eventMsg.BookingOrderId, description);
                    eventPaymentProcessed.MessageType = typeof(PaymentProcessed).ToString();

                    events.AddRange(trackings.PaymentProcessed(eventPaymentProcessed));

                    await _trackingContext.SaveTrackingAsync(eventMsg.BookingOrderId, trackings.OriginalVersion,
                        trackings.Version, events);
                }
                catch(Exception ex)
                {

                }
                
               
            }
        }
    }
}
