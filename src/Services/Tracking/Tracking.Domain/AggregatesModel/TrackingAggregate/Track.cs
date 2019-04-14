using System.Collections.Generic;
using System.Linq;
using Tracking.Domain.Events;
using Tracking.Domain.Model;

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
        public int Version { get;  set; }

        /// <summary>
        /// The original version after handling any commands.
        /// </summary>
        public int OriginalVersion { get; set; }       

        public List<OrderHistory> orderHistory { get; private set; }
        
        public Track()
        {
            OriginalVersion = 0;
            Version = 0;
            orderHistory = new List<OrderHistory>();
        }

        public Track(IEnumerable<EventBase> events)
        {
            orderHistory = new List<OrderHistory>();
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

                     
        //Hanlders , We are building the booking history here
        private IEnumerable<EventBase> Handle(BookingCreated e)
        {
            OrderHistory bookingCreated = new OrderHistory();

            bookingCreated.BookingOrderId = e.BookingId;
            bookingCreated.DateTime = e.Date.ToString();
            bookingCreated.Origion = e.Origin;
            bookingCreated.Destination = e.Destination;
            bookingCreated.Description = e.Description;
            bookingCreated.OrderState = typeof(BookingCreated).ToString();
            
            orderHistory.Add(bookingCreated);

            return new EventBase[] { e };
        }

        private IEnumerable<EventBase> Handle(PaymentProcessed e)
        {
            OrderHistory paymentProcessed = new OrderHistory();

            paymentProcessed.BookingOrderId = e.BookingId;
            paymentProcessed.DateTime = e.Date.ToString();         
            paymentProcessed.Description = e.Description;
            paymentProcessed.OrderState = typeof(PaymentProcessed).ToString();

            orderHistory.Add(paymentProcessed);

            return new EventBase[] { e };
        }

        private IEnumerable<EventBase> Handle(OrderPicked e)
        {
            OrderHistory orderPicked = new OrderHistory();

            orderPicked.BookingOrderId = e.BookingId;
            orderPicked.DateTime = e.Date.ToString();
            orderPicked.Description = e.Description;
            orderPicked.OrderState = typeof(OrderPicked).ToString();

            orderHistory.Add(orderPicked);

            return new EventBase[] { e };
        }

        private IEnumerable<EventBase> Handle(OrderInTransit e)
        {

            OrderHistory orderInTransit = new OrderHistory();

            orderInTransit.BookingOrderId = e.BookingId;
            orderInTransit.DateTime = e.Date.ToString();
            orderInTransit.Description = e.Description;
            orderInTransit.OrderState = typeof(OrderInTransit).ToString();

            orderHistory.Add(orderInTransit);

            return new EventBase[] { e };
        }

        private IEnumerable<EventBase> Handle(OrderDelivered e)
        {
            OrderHistory orderDelivered = new OrderHistory();

            orderDelivered.BookingOrderId = e.BookingId;
            orderDelivered.DateTime = e.Date.ToString();
            orderDelivered.Description = e.Description;
            orderDelivered.OrderState = typeof(OrderDelivered).ToString();
            orderDelivered.SignedBy = e.SignedBy;

            orderHistory.Add(orderDelivered);

            return new EventBase[] { e };
        }
    }
}
