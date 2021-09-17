using System;
using System.Linq;
using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Search.API.Domain;
using Customer = Search.API.Domain.Customer;

namespace Search.API.Installers
{
    public static class ElasticsearchInstaller
    {
        public static IServiceCollection AddElasticsearchInitializer(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);

            var client = new ElasticClient(settings);
            //Map only 0 level of Child property(no recursion mapping)
            var a =client.Indices.Create(defaultIndex, index => index.Map(x => x.AutoMap(0)));
            client.Indices.Create(configuration["elasticsearch:bookIndexName"], c => c
                .Map<Book>(m => m.AutoMap(0)));
            client.Indices.Create(configuration["elasticsearch:customerIndexName"], c => c
                .Map<Customer>(m => m.AutoMap(0)));
            // client.Indices.Create(configuration["elasticsearch:customerIndexName"], c => c
            //     .Map<Customer>(m => m.AutoMap(0).Properties(ps => ps
            //         .Completion(s => s.Name(n => n.NameSuggest))
            //         .Completion(s => s.Name(n => n.SurnameSuggest))
            //         .Completion(s => s.Name(n => n.EmailSuggest)))));
            services.AddSingleton<IElasticClient>(client);
            return services;
        }
    }
}