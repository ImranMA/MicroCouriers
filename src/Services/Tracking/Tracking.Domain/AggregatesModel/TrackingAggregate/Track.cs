using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracking.Domain.Events;

namespace Tracking.Domain.AggregatesModel.TrackingAggregate
{
    public class Track
    {
      
        /// <summary>
        /// Indication whether the aggregate is replaying events (true) or not (false).
        /// </summary>
        private bool IsReplaying { get; set; } = false;

        /// <summary>
        /// The date of the planning (aggregate  Id).
        /// </summary>
        public string Booking { get; private set; }

        /// <summary>
        /// The list of maintenance-jobs for this day. 
        /// </summary>
       // public List<MaintenanceJob> Jobs { get; private set; }

        /// <summary>
        /// The current version of the aggregate after replaying all events in the event-store.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// The original version after handling any commands.
        /// </summary>
        public int OriginalVersion { get; private set; }
       

        public List<string> orderHistory { get; private set; }


        public Track()
        {
            OriginalVersion = 0;
            Version = 0;
            orderHistory = new List<string>();
        }

        public Track(IEnumerable<EventBase> events)
        {
            orderHistory = new List<string>();
            OriginalVersion = 0;
            Version = 0;
            IsReplaying = true;
            foreach (EventBase e in events)
            {
                HandleEvent(e);
                OriginalVersion++;
            }
            IsReplaying = false;
        }


        // <summary>
        /// Handles an event and updates the aggregate version.
        /// </summary>
        /// <param name="e">The event to handle.</param>
        private IEnumerable<EventBase> HandleEvent(dynamic e)
        {
            IEnumerable<EventBase> events = Handle(e);
            Version += events.Count();
            return events;
        }

        public IEnumerable<EventBase> PaymentProcessed(PaymentProcessed e)
        {            
            return HandleEvent(e);
        }

        public IEnumerable<EventBase> BookingAdd(BookingCreated e)
        {   
            return HandleEvent(e);
        }

        public IEnumerable<EventBase> OrderPicked(OrderPicked e)
        {
            return HandleEvent(e);
        }

        public IEnumerable<EventBase> OrderInTransit(OrderInTransit e)
        {
            return HandleEvent(e);
        }

        public IEnumerable<EventBase> OrderDelivered(OrderDelivered e)
        {
            return HandleEvent(e);
        }





        //Hanlders

        private IEnumerable<EventBase> Handle(BookingCreated e)
        {
            orderHistory.Add("Status - " + e.MessageType.ToString() +
           "- Date - " + e.Date.ToShortDateString() + " -Origin " + e.Origin + " - Dest " + e.Destination);

            return new EventBase[] { e };
        }

        private IEnumerable<EventBase> Handle(PaymentProcessed e)
        {
            return new EventBase[] { e };
        }

        private IEnumerable<EventBase> Handle(OrderPicked e)
        {
            orderHistory.Add("Status - " + e.MessageType.ToString() +
                "- Date - " + e.Date.ToShortDateString() +  "  - Desc " + e.Description);
            return new EventBase[] { e };
        }

        private IEnumerable<EventBase> Handle(OrderInTransit e)
        {
            orderHistory.Add("Status - " + e.MessageType.ToString() +
                "- Date - " + e.Date.ToShortDateString() + "  - Desc " + e.Description);

            return new EventBase[] { e };
        }

        private IEnumerable<EventBase> Handle(OrderDelivered e)
        {
            orderHistory.Add("Status - " + e.MessageType.ToString() + 
                "- Date - " + e.Date.ToShortDateString() + " - Received By" + e.SignedBy + "  - Desc " + e.Description);
            return new EventBase[] { e };
        }
    }
}
