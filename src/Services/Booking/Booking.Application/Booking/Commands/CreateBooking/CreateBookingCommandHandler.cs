using Booking.Application.IntegrationEvents;
using Booking.Application.IntegrationEvents.Events;
using Booking.Domain.AggregatesModel.BookingAggregate;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.Application.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, string>
    {
        private readonly IBookingRespository _bookingContext;
        private readonly IBookingIntegrationEventService _bookingIntegrationEventService;
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;
        public CreateBookingCommandHandler(IMediator mediator, IBookingRespository context,
           IEventBus eventBus)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _bookingContext = context;
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            //_bookingIntegrationEventService = bookingIntegrationEventService ?? throw new ArgumentNullException(nameof(bookingIntegrationEventService));
        }

        public async Task<string> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var bookingOrder = new BookingOrder(request.CustomerId, request.Origin,request.Destination);
                        
            foreach (BookingOrderDetails bookdetails in request.BookingDetails)
            {
                bookingOrder.AddBookingDetails(bookingOrder.BookingOrderId,
                    bookdetails.PackageType, bookdetails.PackageDescription, bookdetails.Price);
            }

            var bookingAddIntegrationEvent = new BookingAddIntegrationEvent(bookingOrder.BookingOrderId, bookingOrder.Origin, bookingOrder.Destination);
            await _bookingIntegrationEventService.AddAndSaveEventAsync(bookingAddIntegrationEvent);
            var booking = await _bookingContext.AddAsync(bookingOrder);
            await _bookingIntegrationEventService.PublishEventsThroughEventBusAsync();

            return booking;
        }
    }
}
