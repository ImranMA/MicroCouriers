using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Events
{
    public class OrderProcessed : EventBase
    {
        public OrderProcessed(string bookingId, string description)
        {
            BookingId = bookingId;
            Description = description;
        }
    }
}
