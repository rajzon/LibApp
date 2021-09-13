using EventBus.Messages.Commands;
using EventBus.Messages.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Search.API.Consumers;
using CreateSeededCustomersConsumer = Search.API.Consumers.CreateSeededCustomersConsumer;

namespace Search.API.Installers
{
    public static class EventBusInstaller
    {
        public static IServiceCollection AddEventBusInitializer(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<CreateBookConsumer>();
            services.AddScoped<CreateSeededCustomersConsumer>();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateBookConsumer>();
                x.AddConsumer<AddImageToBookConsumer>();
                x.AddConsumer<CreateSeededCustomersConsumer>();
                x.AddConsumer<GetCustomerInfoConsumer>();
                
                
                x.AddRequestClient<GetCustomerInfo>();
                
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
                    
                    cfg.ReceiveEndpoint(EventBusConstants.CreateSeededCustomersQueue, c =>
                    {
                        c.ConfigureConsumer<CreateSeededCustomersConsumer>(context);
                    });
                    
                    cfg.ReceiveEndpoint(EventBusConstants.GetCustomerInfo, c =>
                    {
                        c.ConfigureConsumer<GetCustomerInfoConsumer>(context);
                    });
                });
                
                x.AddRequestClient<CheckBooksExsitance>();
                
            });
            return services.AddMassTransitHostedService();
        }
    }
}