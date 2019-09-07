using System;
using System.Collections.Generic;
using System.Text;

namespace Shipping.Domain.Entities
{
    public class ShippingsHistory
    {
        public string ShippingsHistoryId { get; set; }

        public ShippingStatus ShippingStatus { get; set; }

        public Shippings Shippings { get; set; }
    }
}
