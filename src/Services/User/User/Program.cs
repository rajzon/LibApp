using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Common;
using GreenPipes.Internals.Mapping;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using User.Data;
using User.Domain;
using Customer = User.Domain.Customer;

namespace User
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var userDbContext = host.Services.GetRequiredService<UserDbContext>();
            userDbContext.Database.Migrate();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var sendEndpointProvider = services.GetRequiredService<ISendEndpointProvider>();
                    var endpoint = sendEndpointProvider.GetSendEndpoint(
                            new Uri($"queue:{EventBusConstants.CreateSeededCustomersQueue}")).Result;
                    
                    var dbContext = services.GetRequiredService<UserDbContext>();
                    var mapper = services.GetRequiredService<IMapper>();
                    var customers = SeedUsers(dbContext);
                    var createSeededCustomersEvent = new CreateSeededCustomers()
                    {
                        Customers = mapper.Map<IEnumerable<CustomerDto>>(customers)
                    };

                    endpoint.Send(createSeededCustomersEvent).Wait();
                }
                catch (Exception e)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "Error occured during migration");
                }
            }
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        private static IEnumerable<Customer> SeedUsers(UserDbContext dbContext)
        {
            var usersToSeed = new List<Customer>()
            {
                new Customer("Ala", "Kot", new Email("ala@kot.pl"), new IdCard("11111", IdentityType.PersonIdCard),
                    IdentityType.PersonIdCard
                    , "Polish", 111222333, DateTime.UtcNow,
                    new Address("Akacjowa 12", "Cracow", new PostCode("29-012"), "Cracow", "Poland"),
                    new AddressCorrespondence("Akacjowa 12 Correspondence", "Cracow Correspondence",
                        new PostCode("29-012"),
                        "Cracow Correspondence", "Poland Correspondence")),
                new Customer("Jan", "Kowalski", new Email("jan@kowalski.pl"),
                    new IdCard("22222", IdentityType.Passport),
                    IdentityType.Passport
                    , "English", 999333222, DateTime.Now,
                    new Address("Glowna 3", "Warsaw", new PostCode("12-012"), "Warsaw", "Poland"),
                    new AddressCorrespondence("Glowna 3 Correspondence", "Warsaw Correspondence",
                        new PostCode("12-012"),
                        "Warsaw Correspondence", "Poland Correspondence"))
            };
            
            dbContext.Customers.AddRange(usersToSeed);
            if (dbContext.SaveChanges() < 1)
                throw new Exception("Error with seeding customers");
            
            return dbContext.Customers.ToList();
        }
    }
}