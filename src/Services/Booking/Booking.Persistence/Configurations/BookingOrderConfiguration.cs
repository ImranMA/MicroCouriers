using Booking.Domain.AggregatesModel.BookingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Persistence.Configurations
{
    public class BookingOrderConfiguration : IEntityTypeConfiguration<BookingOrder>
    {
        public void Configure(EntityTypeBuilder<BookingOrder> builder)
        {
            //defining the navigation property for booking order
            var navigation = builder.Metadata.FindNavigation(nameof(BookingOrder.BookingDetails));          
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            /*builder.HasKey(e => e.BookingOrderId);

            builder.Property(e => e.BookingOrderId).
                HasColumnName("BookingOrderID").HasMaxLength(32);

            builder.Property(e => e.NotificationId)
                .HasColumnName("NotificationID")
                .HasMaxLength(32);

            builder.Property(e => e.CustomerId)
               .HasColumnName("CustomerID")
               .HasMaxLength(32);

            builder.Property(e => e.PaymentId)
              .HasColumnName("PaymentID")
              .HasMaxLength(32);         */
        }
    }
}
