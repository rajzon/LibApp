using EventBus.Messages.Commands;
using EventBus.Messages.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Search.API.Consumers;

namespace Search.API.Installers
{
    public static class EventBusInstaller
    {
        public static IServiceCollection AddEventBusInitializer(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<CreateBookConsumer>();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateBookConsumer>();
                x.AddConsumer<AddImageToBookConsumer>();
                
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostUrl"]);
                    
                    cfg.ReceiveEndpoint(EventBusConstants.CreateBookQueue, c =>
                    {
                        c.ConfigureConsumer<CreateBookConsumer>(context);
                    });
                    cfg.ReceiveEndpoint(EventBusConstants.AddImageToBookQueue, c =>
                    {
                        c.ConfigureConsumer<AddImageToBookConsumer>(context);
                    });
                });
                
                x.AddRequestClient<CheckBooksExsitance>();
                
            });
            return services.AddMassTransitHostedService();
        }
    }
}