using System;
using System.Collections.Generic;

namespace Booking.Domain.AggregatesModel.BookingAggregate
{
    //Aggreegate Model
    public class BookingOrder
    {
        private readonly List<BookingOrderDetail> _bookingDetails;
        public IReadOnlyCollection<BookingOrderDetail> BookingDetails => _bookingDetails;

        public string BookingOrderId { get; set; }
        public string CustomerID { get; set; }
        public string PaymentID { get; set; }
        public string NotificationID { get; set; }

        public BookingOrder()
        {
            _bookingDetails = new List<BookingOrderDetail>();
        }

        public BookingOrder(string paymentID,string notificationID, string bookingOrderId)
        {
            BookingOrderId = bookingOrderId;
            PaymentID = paymentID;
            NotificationID = notificationID;
        }

        public BookingOrder(string customerId)
        {
            BookingOrderId = Guid.NewGuid().ToString();
            CustomerID = customerId;
           _bookingDetails = new List<BookingOrderDetail>();
        }



        public void AddBookingDetails(string BookingOrderId, string PackageType, string Origin,
            string Destination, decimal Price)
        {
            var bookingOrderDetail = new BookingOrderDetail(BookingOrderId, PackageType, Origin, Destination, Price);
            _bookingDetails.Add(bookingOrderDetail);
        }

        public void SetPayment(string paymentID)
        {
            PaymentID = paymentID;
        }
    }

}
