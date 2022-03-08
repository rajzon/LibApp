using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using StockDelivery.API.Data;
using StockDelivery.API.Domain;

namespace StockDelivery.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} [{EventId:u3}]{NewLine}{Exception}")
                .CreateLogger();
            try
            {
                var host = CreateHostBuilder(args).Build();
                var deliveryStockDbContext = host.Services.GetRequiredService<DeliveryStockDbContext>();
                deliveryStockDbContext.Database.Migrate();
                InitializeStockCache(host.Services);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        private static void InitializeStockCache(IServiceProvider services)
        {
            var cache = services.GetRequiredService<IMemoryCache>();
            var stockRepository = services.GetRequiredService<IBookStockRepository>();
            var stocks = stockRepository.GetAllAsync().Result;
            foreach (var stock in stocks)
            {
                Console.WriteLine($"{stock.BookEan13.Code}:{stock.Id}");
                
                if (cache.TryGetValue(stock.BookEan13.Code, out ConcurrentQueue<int> queuedStocks))
                    queuedStocks.Enqueue(stock.Id);
                else
                    cache.Set(stock.BookEan13.Code, new ConcurrentQueue<int>(new[] {stock.Id}));
            }
        }
    }
}