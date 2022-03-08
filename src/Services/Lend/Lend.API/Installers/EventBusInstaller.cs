using EventBus.Messages.Commands;
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lend.API.Installers
{
    public static class EventBusInstaller
    {
        public static IServiceCollection AddEventBusInitializer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddRequestClient<GetCustomerInfo>();
                x.AddRequestClient<GetStockWithBookInfo>();
                x.AddRequestClient<DeleteStocks>();
                x.AddRequestClient<RestoreCachedStock>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostUrl"]);
                });
            });
            return services.AddMassTransitHostedService();
        }
    }
}