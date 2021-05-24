using System;
using System.Linq;
using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Search.API.Domain;

namespace Search.API.Installers
{
    public static class ElasticsearchInstaller
    {
        public static IServiceCollection AddElasticsearchInitializer(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<WeatherForecast>(w =>
                    w.IndexName("weather-forecast"))
                .DefaultMappingFor<Book>(w =>
                    w.IndexName("create-book-event"));

            var client = new ElasticClient(settings);
            //Map only 0 level of Child property(no recursion mapping)
            client.Indices.Create(defaultIndex, index => index.Map(x => x.AutoMap(0)));
            services.AddSingleton<IElasticClient>(client);
            return services;
        }
    }
}