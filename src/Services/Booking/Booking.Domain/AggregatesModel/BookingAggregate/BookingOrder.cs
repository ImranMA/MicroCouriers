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
        public string Origin { get; set; }
        public string Destination { get; set; }

        public bookingStateEnum BookingState { get; set; }

        public string OrderStatus { get; set; }

        public DateTime CreatedDate { get;  set; }

        public DateTime? UpdatedDate { get; set; }

        public BookingOrder()
        {
            _bookingDetails = new List<BookingOrderDetail>();
        }
        

        public BookingOrder(string customerId, string origin, string destination, bookingStateEnum bookingState)
        {
            BookingOrderId = Guid.NewGuid().ToString();
            CustomerID = customerId;
            Origin = origin;
            Destination = destination;
            BookingState = bookingState;
           _bookingDetails = new List<BookingOrderDetail>();
        }


        public void AddBookingDetails(string bookingOrderId, string packageType, 
            string packageDesc, decimal price)
        {
            var bookingOrderDetail = new BookingOrderDetail(bookingOrderId, packageType, packageDesc, price);
            _bookingDetails.Add(bookingOrderDetail);
        }

        public void SetPayment(string paymentID)
        {
            PaymentID = paymentID;
        }
    }

}
