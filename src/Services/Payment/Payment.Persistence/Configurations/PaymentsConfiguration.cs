using Payment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.Persistence.Configurations
{
   public class PaymentsConfiguration : IEntityTypeConfiguration<Payments>
    {
        //Properties are configured here for EFcore to apply them in DB
        public void Configure(EntityTypeBuilder<Payments> builder)
        {
           builder.Property(e => e.Price).HasColumnType("money");
        }

    }
}
