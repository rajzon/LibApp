using System;
using System.Net;
using FluentValidation.AspNetCore;
using Identity.API.Configuration;
using Identity.API.Data;
using Identity.API.Installers;
using Identity.API.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ServicePointManager.Expect100Continue = true;

            var connectionString = _config.GetConnectionString("DefaultConnection");
            var migrationAssembly = typeof(Startup).Assembly.GetName().Name;
            services.AddDbContext<ApplicationDbContext>(config =>
            {
                //SQL Server connection
                config.UseSqlServer(connectionString);
                //config.UseInMemoryDatabase("Memory");
            });
            
            services.AddIdentity<AppUser, IdentityRole<int>>(config =>
                {
                    config.Password.RequiredLength = 8;
                    config.Password.RequireDigit = true;
                    config.Password.RequireNonAlphanumeric = true;
                    config.Password.RequireUppercase = true;
                    config.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });
            
            
            ///IdentityServer - EF Core + SQLServer
             services.AddIdentityServer(opt =>
                 {
                     opt.IssuerUri = "http://identity-service:80";
                 })
                 .AddAspNetIdentity<AppUser>()
                 // this adds the config data from DB (clients, resources)
                 .AddConfigurationStore(options =>
                 {
                     options.ConfigureDbContext = builder =>
                         builder.UseSqlServer(connectionString, 
                             sql => sql.MigrationsAssembly(migrationAssembly));
                 })
                 // this adds the operational data from DB (codes, tokens, consents)
                 .AddOperationalStore(options =>
                 {
                     options.ConfigureDbContext = builder =>
                         builder.UseSqlServer(connectionString, 
                             sql => sql.MigrationsAssembly(migrationAssembly));
                 })
                 .AddDeveloperSigningCredential();


            // services.AddIdentityServer(opt =>
            //     {
            //         opt.IssuerUri = "http://identity-service:80";
            //     })
            //     .AddAspNetIdentity<AppUser>()
            //     .AddInMemoryIdentityResources(Config.GetIdentityResources())
            //     .AddInMemoryApiResources(Config.GetResources())
            //     .AddInMemoryApiScopes(Config.GetScopes())
            //     .AddInMemoryClients(Config.GetClients())
            //     .AddDeveloperSigningCredential();
            
            
            

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddFluentValidation(mvcConfig => mvcConfig.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                });
                
                options.AddPolicy("default2", policy =>
                {
                    policy.WithOrigins("http://localhost:5000")
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                });
                
                options.AddPolicy("default3", policy =>
                {
                    policy.WithOrigins("http://localhost:80")
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                });
                
            });
            //services.AddApiVersioningInitializer();
            services.AddSwaggerInitializer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Identity.API v1");
                c.RoutePrefix = "swagger";
                //var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                // foreach (var description in provider.ApiVersionDescriptions)
                // {
                //     c.SwaggerEndpoint($"/swagger/{description.ApiVersion}/swagger.json", $"Identity.API {description.ApiVersion}");
                //     c.RoutePrefix = "swagger";
                // }
            });
            
            
            app.UseRouting();
            app.UseCors("default");
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}