using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Identity.API.Configuration;
using Identity.API.Data;
using Identity.API.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
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
                SeedDbContexts(services);
                try
                {
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
                    //Seed EF Core contexts for real db approach
                    //SeedDbContexts(services);
                    CreateAdmin(userManager, roleManager);
                    CreateTestEmployee(userManager, roleManager);
                    SeedConfigDbContext(services);
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
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>().UseWebRoot("wwwroot"); });
        
        
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

            // var user = new AppUser
            // {
            //     UserName = "employee",
            //     Email = "employee@example.com"
            // };
            var users = new List<AppUser>() {
                new ()
                {
                    UserName = "employee",
                    Email = "employee@example.com"
                },
                new()
                {
                    UserName = "employee2",
                    Email = "employee2@example.com"
                },
                new ()
                {
                    UserName = "employee3",
                    Email = "employee3@example.com"
                },
                new ()
                {
                    UserName = "employee4",
                    Email = "employee4@example.com"
                } 
            };

            foreach (var user in users)
            {
                var userResult = userManager.CreateAsync(user, "Password1!").GetAwaiter().GetResult();
                if (! userResult.Succeeded)
                    return;
                userManager.AddToRoleAsync(user, "employee").GetAwaiter().GetResult();
                
            }
            userManager.AddClaimsAsync(users.ElementAt(0), new []{ new Claim("book_privilege", "write"),
                new Claim("delivery_privilege", "create-delete") }).GetAwaiter().GetResult();
            userManager.AddClaimsAsync(users.ElementAt(1), new []{ new Claim("book_privilege", "full"),
                new Claim("delivery_privilege", "edit") }).GetAwaiter().GetResult();
            userManager.AddClaimsAsync(users.ElementAt(2), new []{ new Claim("book_privilege", "write"),
                new Claim("delivery_privilege", "full") }).GetAwaiter().GetResult();
            userManager.AddClaimsAsync(users.ElementAt(3), new[] {new Claim("book_privilege", "write")});

        }
        
        
        
        private static void SeedConfigDbContext(IServiceProvider services)
        {
            var configDbContext = services.GetRequiredService<ConfigurationDbContext>();
            configDbContext.Database.Migrate();
            //configDbContext.Database.EnsureCreated();
            if (!configDbContext.IdentityResources.Any())
            {
                foreach (var identityResource in Config.GetIdentityResources())
                {
                    configDbContext.IdentityResources.Add(identityResource.ToEntity());
                }
            }

            if (!configDbContext.ApiResources.Any())
            {
                foreach (var apiResource in Config.GetResources())
                {
                    configDbContext.ApiResources.Add(apiResource.ToEntity());
                }
            }
            
            if (!configDbContext.ApiScopes.Any())
            {
                foreach (var scope in Config.GetScopes())
                {
                    configDbContext.ApiScopes.Add(scope.ToEntity());
                }
            }
            
            if (!configDbContext.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {
                    configDbContext.Clients.Add(client.ToEntity());
                }
            }

            configDbContext.SaveChanges();
        }
        
        private static void SeedDbContexts(IServiceProvider services)
        {
            var appDbContext = services.GetRequiredService<ApplicationDbContext>();
            appDbContext.Database.Migrate();
            //appDbContext.Database.EnsureCreated();
            
            var persistedGrantDbContext = services.GetRequiredService<PersistedGrantDbContext>();
            persistedGrantDbContext.Database.Migrate();
            //persistedGrantDbContext.Database.EnsureCreated();
        }
    }
}