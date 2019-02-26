using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Application.IntegrationEvents
{
    public class PaymentProcessedIntegrationEvent : IntegrationEvent
    {
        public string PaymentId { get; set; }
        public string BookingOrderId { get; set; }
        public string CustomerId { get; set; }
        public int PaymentStatus { get; set; }


        public PaymentProcessedIntegrationEvent(string paymentId, string bookingOrderId,
            string customerId , int paymentStatus)
        {
            PaymentId = paymentId;
            BookingOrderId = bookingOrderId;
            CustomerId = customerId;
            PaymentStatus = paymentStatus;
        }
    }

}
