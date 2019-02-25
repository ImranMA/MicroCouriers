using System.Threading.Tasks;
using Payment.Application.DTO;
using Payment.Application.Interface;

namespace Payment.Infrastructure.PaymentProcessing
{
    public class PaymentExternalGateway : IPaymentExternalGateway
    {
        public Task PaymentProcess(PaymentDTO payment)
        {
            return Task.CompletedTask;
        }
    }
}
