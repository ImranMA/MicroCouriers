using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Payment.Application.Interface;
using Payment.Application.DTO;
using Payment.Domain.Entities;
using Payment.Domain.Interfaces;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
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

        public async Task<string> AddAsync(PaymentDTO payment)
        {
            var resultFromPayment= _gateway.PaymentProcess(payment);
                        
            var paymentEntity = new Payments
            {
                BookingOrderId = payment.BookingOrderId,
                CustomerId = payment.CustomerId,
                Price = payment.Price,
                PaymentStatus = PaymetStatus.Completed //This part is based on the result returned from external gateway
            };

            var paymentRef =  await _context.AddAsync(paymentEntity);

            //Publish the event here
            //Create Integration Event
            var paymentProcessedEvent = new PaymentProcessedIntegrationEvent(paymentRef.PaymentsId,
                paymentRef.BookingOrderId,
                paymentRef.CustomerId, (int)paymentEntity.PaymentStatus);

            _eventBus.Publish(paymentProcessedEvent);

            return paymentRef.PaymentsId;
        }

        public Task<PaymentDTO> FindByIdAsync(string paymentId)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentDTO> UpdateAsync(PaymentDTO payment)
        {
            throw new NotImplementedException();
        }
    }
}
