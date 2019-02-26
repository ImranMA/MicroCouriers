using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Events
{
    public class OrderDelivered : EventBase
    {
        public string SignedBy{ get; set; }

        public OrderDelivered(string bookingId, string description)
        {
            BookingId = bookingId;
            Description = description;
        }

    }
}
