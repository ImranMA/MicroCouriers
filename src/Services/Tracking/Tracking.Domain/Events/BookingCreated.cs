using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Events
{
    public class BookingCreated : EventBase
    {       
        public string Origin { get; set; }
        public string Destination { get; set; }


        public BookingCreated(string bookingId, string description,
         Guid messageId, string messageType, DateTime datetime, string 
            origin, string destination)
        {
            BookingId = bookingId;
            Description = description;
            Origin = origin;
            Destination = destination;
            base.MessageType = messageType;
            base.MessageId = messageId;
            base.Date = datetime;
        }     
    }
}
