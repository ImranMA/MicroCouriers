using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Domain.Entities
{
    public class Payments
    {
        public string PaymentsId { get; set; }

        public string BookingOrderId { get; set; }      

        public decimal Price { get; set; }

        public PaymetStatus paymentStatus { get; set; }
    }
}
