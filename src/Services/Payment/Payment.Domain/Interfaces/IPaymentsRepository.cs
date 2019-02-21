using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Payment.Domain.Entities;
namespace Payment.Domain.Interfaces
{
    public interface IPaymentRepository
    {        
        Task<string> AddAsync( Payments bookingOrder);
        Task<Payments> UpdateAsync(Payments bookingOrder);
        Task<Payments> FindByIdAsync(string bookingOrderId);
    }

}
