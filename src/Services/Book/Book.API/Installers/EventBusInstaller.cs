﻿using Book.API.Consumers;
using EventBus.Messages.Commands;
using EventBus.Messages.Common;
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
                x.AddConsumer<CheckBooksExistanceConsumer>();
                
                x.AddRequestClient<CheckBooksExsitance>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint(EventBusConstants.CheckBooksExistance, e =>
                    {
                        e.ConfigureConsumer<CheckBooksExistanceConsumer>(context);
                    });
                    cfg.Host(configuration["EventBusSettings:HostUrl"]);
                });
            });
            return services.AddMassTransitHostedService();
        }
    }
}