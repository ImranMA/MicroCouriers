using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Model
{
    public class OrderHistory
    {
        public string BookingOrderId { get; set; }

        public string CustomerId { get; set; }

        public string Origion { get; set; }

        public string Destination { get; set; }

        public string OrderState { get; set; }

        public string DateTime { get; set; }

        public string Description { get; set; }

        public string SignedBy { get; set; }

    }
}
