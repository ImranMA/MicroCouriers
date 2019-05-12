using System.Threading.Tasks;
using Payment.Application.DTO;
using Payment.Application.Interface;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.PaymentProcessing
{
    //Simulate the payment Processing
    public class PaymentExternalGateway : IPaymentExternalGateway
    {
        public Task PaymentProcess(Payments payment)
        {
            return Task.CompletedTask;
        }
    }
}
