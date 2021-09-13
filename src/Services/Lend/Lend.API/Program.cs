using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Common;
using Lend.API.Data;
using Lend.API.Domain;
using Lend.API.Domain.Strategies;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lend.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<LendDbContext>();
                    SeedRules(dbContext);
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
        
        private static void SeedRules(LendDbContext dbContext)
        {
            var simpleIntRulesToSeed = new List<SimpleIntRule>()
            {
                new SimpleIntRule("MaxLendDays", "Maximum allowed number of days to lend book", 30,
                    typeof(MaxDaysForLendBookStrategy)),
                new SimpleIntRule("MaxPossibleBooksToLend", "Maximum books to be borrowed by user in particular point of time", 3, typeof(MaxPossibleBooksToLendStrategy))
            };

            var simpleBooleanRulesToSeed = new List<SimpleBooleanRule>()
            {
                new SimpleBooleanRule("CheckDebtor", "If set to true then debtor cannot borrow book", true, typeof(CheckCustomerDebtorStrategy))
            };
            
            dbContext.SimpleIntRules.AddRange(simpleIntRulesToSeed);
            dbContext.SimpleBooleanRules.AddRange(simpleBooleanRulesToSeed);
            if (dbContext.SaveChanges() < 1)
                throw new Exception("Error with seeding customers");
        }
    }
}