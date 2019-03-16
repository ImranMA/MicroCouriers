using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroCourier.Web.DTO
{
    public class BookingOrderDTO
    {
        public string BookingOrderId { get; set; }
        public string CustomerId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }

        public ICollection<BookingOrderDetailDTO> BookingDetails { get; set; }
    }
}
