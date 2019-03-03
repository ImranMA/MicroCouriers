using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Events
{
    public class PaymentProcessed : EventBase
    {     
        public PaymentProcessed(string bookingId, string description)
        {
            BookingId = bookingId;
            Description = description;
        }
    }
}
