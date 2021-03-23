using Book.API.Data;
using Book.API.Domain;
using Book.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Book.API.Installers
{
    public static class BookDbContextInstaller
    {
        public static IServiceCollection AddBookDbContextInitializer(this IServiceCollection services)
        {
            services.AddDbContext<BookDbContext>(config => config.UseInMemoryDatabase("BookService"));
            return services.AddScoped<IBookRepository, BookRepository>();
        }
    }
}