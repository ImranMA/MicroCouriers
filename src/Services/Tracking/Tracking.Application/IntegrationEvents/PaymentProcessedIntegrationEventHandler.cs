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
    public class PaymentProcessedIntegrationEventHandler : IIntegrationEventHandler<PaymentProcessedIntegrationEvent>
    {
        private readonly ITrackingRepository _trackingContext;
        private static List<Type> _assemblyTypes;
        private readonly IEventBus _eventBus;

        public PaymentProcessedIntegrationEventHandler(ITrackingRepository trackingContext , IEventBus eventBus)
        {
            _trackingContext = trackingContext;
            _assemblyTypes = TypeResolver.AssemblyTypes;
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
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
                    else if (eventMsg.PaymentStatus == PaymetStatus.Canceled)
                    {
                        description = "Payment Failed";
                    }
                    else if (eventMsg.PaymentStatus == PaymetStatus.Pending)
                    {
                        description = "Payment Pending";
                    }

                    var messageType = _assemblyTypes
                      .Where(t => t.Name.Contains("PaymentProcessed")).FirstOrDefault().
                      Name.ToString();

                    PaymentProcessed eventPaymentProcessed = new
                        PaymentProcessed(eventMsg.BookingOrderId, description, eventMsg.Id, messageType, eventMsg.CreationDate);


                    events.AddRange(trackings.PaymentProcessed(eventPaymentProcessed));

                    await _trackingContext.SaveTrackingAsync(eventMsg.BookingOrderId, trackings.OriginalVersion,
                        trackings.Version, events);

                    //Publish the event here
                    //Create Integration Event
                    var orderStatusChanged = new OrderStatusChangedIntegrationEvent(eventMsg.BookingOrderId , "PaymentProcessed");
                    _eventBus.Publish(orderStatusChanged);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
