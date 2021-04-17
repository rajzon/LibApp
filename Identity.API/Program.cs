using System;
using System.Linq;
using System.Security.Claims;
using Identity.API.Data;
using Identity.API.Models;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
                    //Seed EF Core contexts for real db approach
                    //SeedDbContexts(services);
                    CreateAdmin(userManager, roleManager);
                    CreateTestEmployee(userManager, roleManager);

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
        
        
        private static void CreateAdmin(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            if (userManager.Users.Any(u => u.Email.Equals("admin@example.com"))) 
                return;
            
            var user = new AppUser
            {
                UserName = "admin",
                Email = "admin@example.com"
            };
            var roleResult = roleManager.CreateAsync(new IdentityRole<int> {Name = "admin"}).GetAwaiter().GetResult();
            if (! roleResult.Succeeded)
                return;
            
            
            var userResult = userManager.CreateAsync(user, "Password1!").GetAwaiter().GetResult();
            if (! userResult.Succeeded)
                return;
            
            userManager.AddToRoleAsync(user, "admin").GetAwaiter().GetResult();
            

        }
        
        
        private static void CreateTestEmployee(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            if (userManager.Users.Any(u => u.Email.Equals("employee@example.com")))
                return;

            var roleResult = roleManager.CreateAsync(new IdentityRole<int> {Name = "employee"}).GetAwaiter().GetResult();
            if (! roleResult.Succeeded)
                return;

            var user = new AppUser
            {
                UserName = "employee",
                Email = "employee@example.com"
            };
            
            var userResult = userManager.CreateAsync(user, "Password1!").GetAwaiter().GetResult();
            if (! userResult.Succeeded)
                return;
            userManager.AddToRoleAsync(user, "employee").GetAwaiter().GetResult();
            userManager.AddClaimAsync(user, new Claim("book_privilege", "write")).GetAwaiter().GetResult();
        }
        
        
        private static void SeedDbContexts(IServiceProvider services)
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
    }
}