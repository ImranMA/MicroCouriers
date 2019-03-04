using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Events
{
    public class EventBase
    {
        public  Guid MessageId;

        public  string MessageType;

        public string BookingId { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
