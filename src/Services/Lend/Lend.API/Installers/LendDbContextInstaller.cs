using Lend.API.Data;
using Lend.API.Data.Repositories;
using Lend.API.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lend.API.Installers
{
    public static class LendDbContextInstaller
    {
        public static IServiceCollection AddLendDbContextInstaller(this IServiceCollection services, IConfiguration cfg)
        {
            var connectionString = cfg.GetConnectionString("DefaultConnection");
            services.AddDbContext<LendDbContext>(config =>
            {
                //config.UseInMemoryDatabase("LendService"));
                config.UseSqlServer(connectionString);

            });
            services.AddScoped<ILendedBasketRepository, LendedBasketRepository>();
            services.AddScoped<ISimpleBooleanRuleRepository, SimpleBooleanRuleRepository>();
            return services.AddScoped<ISimpleIntRuleRepository, SimpleIntRuleRepository>();
        }
    }
}