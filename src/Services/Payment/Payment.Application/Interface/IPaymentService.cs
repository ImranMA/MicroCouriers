using Payment.Application.DTO;
using System.Threading.Tasks;

namespace Payment.Application.Interface
{
    public interface IPaymentService
    {
        Task<string> AddAsync(PaymentDTO payment);
        Task<PaymentDTO> UpdateAsync(PaymentDTO payment);
        Task<PaymentDTO> FindByIdAsync(string paymentId);
    }
}
