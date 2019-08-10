using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Entities;

namespace Shipping.Persistence
{
    public class ShippingDbContext : DbContext
    {

        public ShippingDbContext(DbContextOptions<ShippingDbContext> options)
            : base(options)
        {

        }

        public DbSet<Shippings> Shippings { get; set; }

        public DbSet<ShippingsHistory> ShippingsHistory { get; set; }

        //One-to-Many RelationShipSetup
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShippingsHistory>()
                .HasOne(p => p.Shippings)
                .WithMany(b => b.ShippingHistory);
        }

    }
}
