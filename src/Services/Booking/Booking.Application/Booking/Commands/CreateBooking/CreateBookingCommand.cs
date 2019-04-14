using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Booking.Application.Booking.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<string>
    {
        [Required]
        public readonly ICollection<BookingOrderDetails> BookingDetails;

        public CreateBookingCommand(string bookingOrderId,string customerId)
        {           
            CustomerId = customerId;
            BookingDetails = new List<BookingOrderDetails>();
        }

        [Required]
        [MinLength(5, ErrorMessage = "Origin is required")]
        public string Origin { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Destination is required")]
        public string Destination { get; set; }

        //[Required]
        public string CustomerId { get; set; }
        
    }

    public class BookingOrderDetails
    {
        public string PackageType { get; set; }
        public string PackageDescription { get; set; }    
        public decimal Price { get; set; }
    }
}
