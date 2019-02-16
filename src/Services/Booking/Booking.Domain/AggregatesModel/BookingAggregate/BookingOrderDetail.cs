using System;

namespace Booking.Domain.AggregatesModel.BookingAggregate
{

    public class BookingOrderDetail
    {
        public BookingOrderDetail()
        {

        }

        public string BookingOrderDetailId { get; set; }
        public string BookingOrderId { get; set; }
        public string PackageType { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Price { get; set; }

        public BookingOrderDetail(string bookingOrderId, string packageType, string origin,
           string destination, decimal price)
        {
            BookingOrderDetailId = Guid.NewGuid().ToString();
            BookingOrderId = bookingOrderId;
            PackageType = packageType;
            Destination = destination;
            Origin = origin;
            Price = price;
        }

    }
}
