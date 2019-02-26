using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Events
{
    public class OrderOnBoard : EventBase
    {
        public DateTime 
            ExpectedDelivery  { get; set; }

        public OrderOnBoard(string bookingId, string description)
        {
            BookingId = bookingId;
            Description = description;
        }
    }
}
