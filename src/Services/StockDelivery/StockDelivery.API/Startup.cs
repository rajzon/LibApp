using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Serilog;
using StockDelivery.API.AuthHandler;
using StockDelivery.API.Installers;
using StockDelivery.API.Mappings;

namespace StockDelivery.API
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
            ServicePointManager.Expect100Continue = true;
            IdentityModelEventSource.ShowPII = true;
            
            
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = "http://identity-service:80";
                    config.Audience = "stock_delivery_api";
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
                config.AddPolicy("delivery-create-delete", policyBuilder =>
                {
                    policyBuilder.RequireRole("employee")
                        .RequireClaim("delivery_privilege", "create-delete", "full");
                });
                config.AddPolicy("delivery-edit", policyBuilder =>
                {
                    policyBuilder.RequireRole("employee")
                        .RequireClaim("delivery_privilege", "edit", "create-delete", "full");
                });

            });
            
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationHandler, AdminAuthHandler>();
            
            
            services.AddApiVersioningInitializer();
            services.AddSwaggerInitializer();
            services.AddEventBusInitializer(Configuration);
            services.AddMediatR(typeof(Startup));
            services.AddDeliveryStockContextInitializer();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            
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
                    c.SwaggerEndpoint($"/swagger/{description.ApiVersion}/swagger.json", $"StockDelivery.API {description.ApiVersion}");
                    c.RoutePrefix = "swagger";
                }
            });

            app.UseHttpsRedirection();
            
            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader().AllowCredentials().AllowAnyMethod().WithOrigins("http://localhost:4200"));

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}