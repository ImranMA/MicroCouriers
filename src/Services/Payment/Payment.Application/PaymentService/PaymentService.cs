using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Payment.Application.Interface;
using Payment.Application.DTO;
using Payment.Domain.Entities;
using Payment.Domain.Interfaces;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Abstractions;
using Payment.Application.IntegrationEvents;

namespace Payment.Application.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _context;
        private readonly IPaymentExternalGateway _gateway;
        private readonly IEventBus _eventBus;

        public PaymentService(IPaymentRepository dbContext , IPaymentExternalGateway gateway
            , IEventBus eventBus)
        {
            _context = dbContext;
            _gateway = gateway;
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<string> AddAsync(Payments payment)
        {
            var resultFromPayment= _gateway.PaymentProcess(payment);
            payment.PaymentStatus = PaymetStatus.Completed;
          
            var paymentRef =  await _context.AddAsync(payment);

            //Publish the event here
            //Create Integration Event
            var paymentProcessedEvent = new PaymentProcessedIntegrationEvent(paymentRef.PaymentsId,
                paymentRef.BookingOrderId,
                paymentRef.CustomerId, (int)payment.PaymentStatus);

            _eventBus.Publish(paymentProcessedEvent);

            return paymentRef.PaymentsId;
        }

        public Task<Payments> FindByIdAsync(string paymentId)
        {
            throw new NotImplementedException();
        }

        public Task<Payments> UpdateAsync(Payments payment)
        {
            throw new NotImplementedException();
        }
    }
}
