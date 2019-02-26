using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Events
{
    public class BookingCreated : EventBase
    {       
        public string Origin { get; set; }
        public string Destination { get; set; }


        public BookingCreated(string bookingId, string origin, string destination)
        {
            BookingId = bookingId;
            Origin = origin;
            Destination = destination;
        }
    }
}
