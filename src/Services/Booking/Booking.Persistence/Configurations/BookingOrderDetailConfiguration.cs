using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Persistence.Configurations
{
    public class BookingOrderDetailConfiguration : IEntityTypeConfiguration<BookingOrderDetail>
    {
        public void Configure(EntityTypeBuilder<BookingOrderDetail> builder)
        {
            builder.Property(e => e.Price).HasColumnType("money");
        }

    }
}
