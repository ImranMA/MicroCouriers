using System;
using System.Collections.Generic;
using System.Text;

namespace ReadModel.AzFn.Events
{
    public class OrderStatusChangedIntegrationEvent
    {
        public string BookingId { get; set; }
        public string CurrentStatus { get; set; }
        public OrderStatusChangedIntegrationEvent(string bookingId, string currentStatus)
        {
            CurrentStatus = currentStatus;
            BookingId = bookingId;
        }
    }
}
