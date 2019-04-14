using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicroCourier.Web.DTO
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

    public enum PaymetStatusDTO
    {
        Pending = 0,
        Completed = 1,
        Canceled = 2
    }
}
