using Payment.Domain.Entities;
using Payment.Persistence;
using Payment.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Tests.UnitTests.Application.Infrastructure
{
    public class PaymentContextFactory
    {
        public static PaymentsRepository Create(PaymentDbContext context)
        {
            context.Database.EnsureCreated();
            var paymentRepo = new PaymentsRepository(context);          
            return paymentRepo;
        }

        public static void Destroy(PaymentDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
