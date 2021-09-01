using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using User.Data;

namespace User.Installers
{
    public static class UserDbContextInstaller
    {
        public static IServiceCollection AddBookDbContextInitializer(this IServiceCollection services)
        {
            return services.AddDbContext<UserDbContext>(config => config.UseInMemoryDatabase("UserService"));
        }
    }
}