using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Book.API.Installers
{
    public static class SwaggerInstaller
    {
        public static IServiceCollection AddSwaggerInitializer(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.ApiVersion.ToString(), new OpenApiInfo()
                    {
                        Title = "Book API",
                        Version = description.ApiVersion.ToString()
                    });
                }
            });
        }
    }
}