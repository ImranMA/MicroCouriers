

namespace Booking.Domain.Entities
{
    public class BookingOrderDetail
    {
        public string BookingOrderDetailId { get; set; }
        public string BookingOrderId { get; set; }
        public string PackageType { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Price { get; set; }
    }
}
