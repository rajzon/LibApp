using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using User.Data;

namespace User.Installers
{
    public static class UserDbContextInstaller
    {
        public static IServiceCollection AddBookDbContextInitializer(this IServiceCollection services, IConfiguration cfg)
        {
            var connectionString = cfg.GetConnectionString("DefaultConnection");
            return services.AddDbContext<UserDbContext>(config =>
            {
                // config.UseInMemoryDatabase("UserService");
                config.UseSqlServer(connectionString);
            });
        }
    }
}