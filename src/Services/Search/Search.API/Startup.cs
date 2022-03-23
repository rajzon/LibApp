using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Search.API.Application.Services;
using Search.API.AuthHandlers;
using Search.API.Infrastructure.Data;
using Search.API.Installers;
using Search.API.Mappings;
using Serilog;

namespace Search.API
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
                    config.Audience = "search_api";
                    config.RequireHttpsMetadata = false;
                    
                    config.TokenValidationParameters.ValidIssuers = new[]
                    {
                        "http://localhost:8000",
                        "https://localhost:8001",
                        "http://identity-service:80"
                    };
                });

            services.AddAuthorization();
            services.AddSingleton<IAuthorizationHandler, AdminAuthHandler>();
            
            
            services.AddElasticsearchInitializer(Configuration);
            
            services.AddScoped<ISearchRepository, SearchRepository>();
            
            services.AddEventBusInitializer(Configuration);
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddApiVersioningInitializer();
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
                    c.SwaggerEndpoint($"/swagger/{description.ApiVersion}/swagger.json", $"Search.API {description.ApiVersion}");
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