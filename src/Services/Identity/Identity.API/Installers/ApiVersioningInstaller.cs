using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Installers
{
    public static class ApiVersioningInstaller
    {
        public static IServiceCollection AddApiVersioningInitializer(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}