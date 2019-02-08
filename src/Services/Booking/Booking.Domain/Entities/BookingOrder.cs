using System.Collections.Generic;

namespace Booking.Domain.Entities
{
    public class BookingOrder
    {
        public string BookingOrderId { get; set; }        
        public string CustomerId { get; set; }        
        public string PaymentId { get; set;}
        public string NotificationId { get; set;}        

        public ICollection<BookingOrderDetail> BookingDetails { get; private set; }
    }
}
