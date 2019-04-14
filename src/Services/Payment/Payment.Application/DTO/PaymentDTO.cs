using Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Application.DTO
{
    public class PaymentDTO
    {
        public string PaymentsId { get; set; }

        [Required]
        public string BookingOrderId { get; set; }

        public string CustomerId { get; set; }

        [Required]
        public decimal Price { get; set; }

        public PaymetStatusDTO PaymentStatus { get; set; }
    }
}
