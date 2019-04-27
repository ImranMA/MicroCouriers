using Booking.Domain.AggregatesModel.BookingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Persistence.Configurations
{
    public class BookingOrderDetailConfiguration : IEntityTypeConfiguration<BookingOrderDetail>
    {
        //Property configurations are applied here for EFcore to apply
        public void Configure(EntityTypeBuilder<BookingOrderDetail> builder)
        {
            builder.Property(e => e.Price).HasColumnType("money");
        }

    }
}
