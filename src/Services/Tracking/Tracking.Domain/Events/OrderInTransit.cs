using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Events
{
    public class OrderInTransit : EventBase
    {
        public OrderInTransit(string bookingId, string description,
            Guid messageId, string messageType, DateTime datetime)
        {
            BookingId = bookingId;
            Description = description;
            base.MessageType = messageType;
            base.MessageId = messageId;
            base.Date = datetime;
        }
    }
}
