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
        }

        public Track(IEnumerable<EventBase> events)
        {
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

        private IEnumerable<EventBase> Handle(BookingCreated e)
        {           
            return new EventBase[] { e };
        }

        private IEnumerable<EventBase> Handle(PaymentProcessed e)
        {
            return new EventBase[] { e };
        }
    }
}
