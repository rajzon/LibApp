using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Book.API.Installers
{
    public static class EventBusInstaller
    {
        public static IServiceCollection AddEventBusInitializer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostUrl"]);
                });
            });
            return services.AddMassTransitHostedService();
        }
    }
}