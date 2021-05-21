using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

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
                    w.IndexName("weather-forecast"));

            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
            //Map only 0 level of Child property(no recursion mapping)
            client.Indices.Create(defaultIndex, index => index.Map(x => x.AutoMap(0)));

            return services;
        }
    }
}