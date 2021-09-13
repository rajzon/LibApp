using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lend.API.AuthHandler;
using Lend.API.Domain;
using Lend.API.Domain.Strategies;
using Lend.API.Installers;
using Lend.API.Mappings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Lend.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = "http://identity-service:80";
                    config.Audience = "lend_api";
                    config.RequireHttpsMetadata = false;
                    
                    config.TokenValidationParameters.ValidIssuers = new[]
                    {
                        "http://localhost:8000",
                        "https://localhost:8001",
                        "http://identity-service:80"
                    };
                });
            
            services.AddAuthorization(config =>
            {
            });
            
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationHandler, AdminAuthHandler>();
            services.AddMemoryCache();

            services.AddScoped<IStrategy<SimpleIntRule>, MaxPossibleBooksToLendStrategy>();
            services.AddScoped<IStrategy<SimpleBooleanRule>, CheckCustomerDebtorStrategy>();
            
            services.AddLendDbContextInstaller();
            
            services.AddEventBusInitializer(Configuration);
            services.AddApiVersioningInitializer();
            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddSwaggerInitializer();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.ApiVersion}/swagger.json", $"Lend.API {description.ApiVersion}");
                    c.RoutePrefix = "swagger";
                }
            });
            

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader().AllowCredentials().AllowAnyMethod().WithOrigins("http://localhost:4200"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}