using Microsoft.EntityFrameworkCore;
using StockDelivery.API.Data.Configurations;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Data
{
    public class DeliveryStockDbContext : DbContext, IUnitOfWork
    {
        public DbSet<ActiveDelivery> ActiveDeliveries { get; set; }


        public DeliveryStockDbContext(DbContextOptions<DeliveryStockDbContext> options)
            :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ActiveDeliveryConfiguration());
            modelBuilder.ApplyConfiguration(new ActiveDeliveryItemConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }

    }
}