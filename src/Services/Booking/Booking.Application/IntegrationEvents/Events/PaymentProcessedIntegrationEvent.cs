using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Application.IntegrationEvents.Events
{
    public class PaymentProcessedIntegrationEvent : IntegrationEvent
    {
        public string PaymentId { get; set; }
        public string BookingOrderId { get; set; }
        public string CustomerId { get; set; }
        public PaymetStatus PaymentStatus { get; set; }


        public PaymentProcessedIntegrationEvent(string paymentId, string bookingOrderId,
            string customerId , PaymetStatus paymentStatus)
        {
            PaymentId = paymentId;
            BookingOrderId = bookingOrderId;
            CustomerId = customerId;
            PaymentStatus = paymentStatus;
        }
    }

    public enum PaymetStatus
    {
        Pending = 0,
        Completed = 1,
        Canceled = 2
    }

}
