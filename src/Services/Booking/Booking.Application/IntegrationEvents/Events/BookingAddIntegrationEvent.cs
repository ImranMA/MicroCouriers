using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Application.IntegrationEvents.Events
{
    public class BookingAddIntegrationEvent : IntegrationEvent
    {
        public string BookingId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }


        public BookingAddIntegrationEvent(string bookingId, string origin, string destination )
        {
            BookingId = bookingId;
            Origin = origin;
            Destination = destination;
        }
    }
}
