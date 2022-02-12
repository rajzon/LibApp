using System.Net;
using Book.API.AuthHandlers;
using Book.API.Behaviors;
using Book.API.Installers;
using Book.API.Mappings;
using Book.API.Middleware;
using Book.API.Services;
using Book.API.Settings;
using FluentValidation.AspNetCore;
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

namespace Book.API
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
                    config.Audience = "book_api";
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
                config.AddPolicy("book-write", policyBuilder =>
                {
                    policyBuilder.RequireRole("employee")
                        .RequireClaim("book_privilege", "write");
                });
                
                config.AddPolicy("book-edit", policyBuilder =>
                {
                    policyBuilder.RequireRole("employee")
                        .RequireClaim("book_privilege", "edit", "write");
                });

            });
            
            
            services.AddControllers()
                .AddFluentValidation(mvcConfig => mvcConfig.RegisterValidatorsFromAssemblyContaining<Startup>());
            
            
            
            services.AddBookDbContextInitializer(Configuration);
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddMediatR(typeof(Startup));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            
            
            services.AddApiVersioningInitializer();
            services.AddSwaggerInitializer();

            services.AddEventBusInitializer(Configuration); 
            
            
            
            services.AddCors();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            

            services.AddSingleton<IAuthorizationHandler, AdminAuthHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<HttpRequestBodyMiddleware>();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(c =>
                // {
                //     var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                //     foreach (var description in provider.ApiVersionDescriptions)
                //     {
                //         c.SwaggerEndpoint($"/swagger/{description.ApiVersion}/swagger.json", $"Book.API {description.ApiVersion}");
                //         c.RoutePrefix = "swagger";
                //     }
                // });
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.ApiVersion}/swagger.json", $"Book.API {description.ApiVersion}");
                    c.RoutePrefix = "swagger";
                }
            });
            

            // app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            
            
            
            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader().AllowCredentials().AllowAnyMethod().WithOrigins("http://localhost:4200"));
            
            app.UseAuthentication();
            app.UseAuthorization();




            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}