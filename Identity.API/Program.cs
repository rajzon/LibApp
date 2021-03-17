using System;
using Identity.API.Data;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Identity.API
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
                    var configDbContext = services.GetRequiredService<ConfigurationDbContext>();
                    configDbContext.Database.Migrate();
                    configDbContext.Database.EnsureCreated();
                
                    var appDbContext = services.GetRequiredService<ApplicationDbContext>();
                    appDbContext.Database.Migrate();
                    appDbContext.Database.EnsureCreated();
                
                    var persistedGrantDbContext = services.GetRequiredService<PersistedGrantDbContext>();
                    persistedGrantDbContext.Database.Migrate();
                    persistedGrantDbContext.Database.EnsureCreated();

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
    }
}