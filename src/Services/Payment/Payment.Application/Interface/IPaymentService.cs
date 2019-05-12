using Payment.Application.DTO;
using Payment.Domain.Entities;
using System.Threading.Tasks;

namespace Payment.Application.Interface
{
    public interface IPaymentService
    {
        Task<string> AddAsync(Payments payment);
        Task<Payments> UpdateAsync(Payments payment);
        Task<Payments> FindByIdAsync(string paymentId);
    }
}
