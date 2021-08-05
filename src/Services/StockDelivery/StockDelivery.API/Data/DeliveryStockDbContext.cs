using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockDelivery.API.Data.Configurations;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Data
{
    public class DeliveryStockDbContext : DbContext, IUnitOfWork
    {
        public DbSet<ActiveDelivery> ActiveDeliveries { get; set; }
        public DbSet<CancelledDelivery> CancelledDeliveries { get; set; }
        public DbSet<CompletedDelivery> CompletedDeliveries { get; set; }
        public DbSet<BookStock> BookStocks { get; set; }

        private readonly IMediator _mediator;
        
        public DeliveryStockDbContext(DbContextOptions<DeliveryStockDbContext> options, IMediator mediator)
            :base(options)
        {
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompletedDeliveryConfiguration());
            modelBuilder.ApplyConfiguration(new ActiveDeliveryConfiguration());
            modelBuilder.ApplyConfiguration(new CancelledDeliveryConfiguration());
            modelBuilder.ApplyConfiguration(new BookStockConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> SaveChangesAndDoEventsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);
            
            return await base.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}