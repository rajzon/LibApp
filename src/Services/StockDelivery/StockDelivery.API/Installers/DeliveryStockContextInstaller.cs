using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockDelivery.API.Data;
using StockDelivery.API.Data.Repositories;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Installers
{
    public static class DeliveryStockContextInstaller
    {
        public static IServiceCollection AddDeliveryStockContextInitializer(this IServiceCollection services, IConfiguration cfg)
        {
            var connectionString = cfg.GetConnectionString("DefaultConnection");
            services.AddDbContext<DeliveryStockDbContext>(config =>
            {
                //config.UseInMemoryDatabase("DeliveryStockService");
                config.UseSqlServer(connectionString);
            });
            services.AddScoped<ICancelledDeliveryRepository, CancelledDeliveryRepository>();
            services.AddScoped<IBookStockRepository, BookStockRepository>();
            services.AddScoped<ICompletedDeliveryRepository, CompletedDeliveryRepository>();
            return services.AddScoped<IActiveDeliveryRepository, ActiveDeliveryRepository>();
        }
    }
}