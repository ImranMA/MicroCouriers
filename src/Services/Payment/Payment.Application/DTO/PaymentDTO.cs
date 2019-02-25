using Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Application.DTO
{
    public class PaymentDTO
    {
        public string PaymentsId { get; set; }

        public string BookingOrderId { get; set; }

        public string CustomerId { get; set; }        

        public decimal Price { get; set; }

        public PaymetStatusDTO PaymentStatus { get; set; }
    }
}
