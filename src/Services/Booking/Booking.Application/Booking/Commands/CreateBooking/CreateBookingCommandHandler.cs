using Booking.Application.IntegrationEvents;
using Booking.Application.IntegrationEvents.Events;
using Booking.Domain.AggregatesModel.BookingAggregate;
using Booking.Domain.Booking;
using MediatR;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.Application.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, string>
    {
        private readonly IBookingRespository _bookingContext;                
        private readonly IEventBus _eventBus;

        //private readonly IBookingIntegrationEventService _bookingIntegrationEventService;

        public CreateBookingCommandHandler(IBookingRespository context,
           IEventBus eventBus)
        {
            _bookingContext = context;
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
           // _bookingIntegrationEventService = bookingIntegrationEventService ?? throw new ArgumentNullException(nameof(bookingIntegrationEventService));
        }

        public async Task<string> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {

            //Create Agreegate Root
            var bookingOrder = new BookingOrder(request.CustomerId, request.Origin,request.Destination 
                , bookingStateEnum.Pending);                        

            //Add Order Details
            foreach (BookingOrderDetails bookdetails in request.BookingDetails)
            {
                bookingOrder.AddBookingDetails(bookingOrder.BookingOrderId,
                    bookdetails.PackageType, bookdetails.PackageDescription, bookdetails.Price);
            }

            //Create Integration Event
            var bookingAddIntegrationEvent = new BookingAddIntegrationEvent(bookingOrder.BookingOrderId, 
                bookingOrder.Origin,
                bookingOrder.Destination);
          
            
            //Save the Data in local DB
            var booking = await _bookingContext.AddAsync(bookingOrder);

            //Publish Event to Service Bus Topic
            _eventBus.Publish(bookingAddIntegrationEvent);            

            //Return Booking Ref
            return booking;
        }
    }
}
