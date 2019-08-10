using System;
using System.Collections.Generic;
using System.Text;

namespace Shipping.Domain.Entities
{
    public class Shippings
    {
        public string ShippingsId { get; set; }

        public string BookingOrderId { get; set; }

        public string CustomerId { get; set; }

        public ShippingStatus ShippingStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public ICollection<ShippingsHistory> ShippingHistory { get; set; }
    }
}
