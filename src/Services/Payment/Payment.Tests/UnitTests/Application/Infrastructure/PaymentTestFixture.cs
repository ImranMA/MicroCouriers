using Microsoft.EntityFrameworkCore;
using Payment.Domain.Interfaces;
using Payment.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Payment.Tests.UnitTests.Application.Infrastructure
{
    public class PaymentTestFixture : IDisposable
    {
        public PaymentDbContext Context { get; private set; }
        public IPaymentRepository repo { get; private set; }

        public PaymentTestFixture()
        {
            var options = new DbContextOptionsBuilder<PaymentDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;

            Context = new PaymentDbContext(options);
            repo = PaymentContextFactory.Create(Context);
        }

        public void Dispose()
        {
            PaymentContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("PaymentCollection")]
    public class CommandCollection : ICollectionFixture<PaymentTestFixture> { }
}
