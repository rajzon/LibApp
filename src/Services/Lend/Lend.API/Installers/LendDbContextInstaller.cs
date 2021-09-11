using Lend.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lend.API.Installers
{
    public static class LendDbContextInstaller
    {
        public static IServiceCollection AddLendDbContextInstaller(this IServiceCollection services)
        {
            return services.AddDbContext<LendDbContext>(config => 
                config.UseInMemoryDatabase("LendService"));
        }
    }
}