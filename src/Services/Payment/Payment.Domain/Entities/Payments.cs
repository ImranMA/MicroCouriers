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

        public string CustomerId { get; set; }

        public PaymetStatus PaymentStatus { get; set; }

        public DateTime CreatedDate { get;  set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
