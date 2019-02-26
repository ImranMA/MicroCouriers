using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Events
{
    public class OrderPicked  : EventBase
    {
        public OrderPicked(string bookingId, string description)
        {
            BookingId = bookingId;           
            Description = description;
        }
    }
}
