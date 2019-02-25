using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Payment.Domain.Entities;
namespace Payment.Domain.Interfaces
{
    public interface IPaymentRepository
    {        
        Task<Payments> AddAsync( Payments payment);
        Task<Payments> UpdateAsync(Payments payment);
        Task<Payments> FindByIdAsync(string paymentId);
    }

}
