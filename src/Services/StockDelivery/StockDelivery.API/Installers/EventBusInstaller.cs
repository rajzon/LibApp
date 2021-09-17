using EventBus.Messages.Commands;
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockDelivery.API.Consumers;

namespace StockDelivery.API.Installers
{
    public static class EventBusInstaller
    {
        public static IServiceCollection AddEventBusInitializer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CheckStocksExistanceConsumer>();
                x.AddConsumer<GetStockWithBookInfoConsumer>();
                
                x.AddRequestClient<CheckBooksExsitance>();
                x.AddRequestClient<GetBooksInfo>();
                x.AddRequestClient<GetBookInfo>();
                
                x.AddRequestClient<CheckStocksExistance>();
                x.AddRequestClient<GetStockWithBookInfo>();
                
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint(EventBusConstants.CheckStocksExistance, e =>
                    {
                        e.ConfigureConsumer<CheckStocksExistanceConsumer>(context);
                        
                    });
                    
                    cfg.ReceiveEndpoint(EventBusConstants.GetStockWithBookInfo, e =>
                    {
                        e.ConfigureConsumer<GetStockWithBookInfoConsumer>(context);
                        
                    });

                    
                    cfg.Host(configuration["EventBusSettings:HostUrl"]);
                });
            });
            return services.AddMassTransitHostedService();
        }
    }
}