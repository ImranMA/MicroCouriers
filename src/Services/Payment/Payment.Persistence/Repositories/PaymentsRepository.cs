using Payment.Domain.Entities;
using Payment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Persistence.Repositories
{

    public class PaymentsRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;


        public PaymentsRepository(PaymentDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Payments> AddAsync(Payments payment)
        {
            payment.CreatedDate = DateTime.Now;

            _context.Set<Payments>().Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<Payments> FindByIdAsync(string paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            return payment;
        }

        public async Task<Payments> UpdateAsync(Payments payment)
        {

            payment.UpdatedDate = DateTime.Now;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return payment;
        }
    }
}
