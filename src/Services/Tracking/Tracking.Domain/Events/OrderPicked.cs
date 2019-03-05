using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Events
{
    public class OrderPicked  : EventBase
    {
        public OrderPicked(string bookingId, string description,
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
