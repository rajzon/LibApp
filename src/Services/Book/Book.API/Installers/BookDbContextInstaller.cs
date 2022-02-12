using Book.API.Data;
using Book.API.Data.Decorators;
using Book.API.Data.Repositories;
using Book.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Book.API.Installers
{
    public static class BookDbContextInstaller
    {
        public static IServiceCollection AddBookDbContextInitializer(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<BookDbContext>(config =>
            {
                //config.UseInMemoryDatabase("BookService");
                config.UseSqlServer(connectionString);
            });
            services.AddScoped<ILanguageRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<BookDbContext>();
                var logger = sp.GetRequiredService<ILogger<LanguageRepositoryLoggingDecorator>>();

                var service = new LanguageRepository(dbContext);
                return new LanguageRepositoryLoggingDecorator(service, logger);
            });
            
            services.AddScoped<IAuthorRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<BookDbContext>();
                var logger = sp.GetRequiredService<ILogger<AuthorRepositoryLoggingDecorator>>();

                var service = new AuthorRepository(dbContext);
                return new AuthorRepositoryLoggingDecorator(service, logger);
            });
            
            services.AddScoped<IPublisherRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<BookDbContext>();
                var logger = sp.GetRequiredService<ILogger<PublisherRepositoryLoggingDecorator>>();

                var service = new PublisherRepository(dbContext);
                return new PublisherRepositoryLoggingDecorator(service, logger);
            });
            
            
            services.AddScoped<ICategoryRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<BookDbContext>();
                var logger = sp.GetRequiredService<ILogger<CategoryRepositoryLoggingDecorator>>();

                var service = new CategoryRepository(dbContext);
                return new CategoryRepositoryLoggingDecorator(service, logger);
            });
            
            return services.AddScoped<IBookRepository>(sp =>
            {
                var dbContext = sp.GetRequiredService<BookDbContext>();
                var logger = sp.GetRequiredService<ILogger<BookRepositoryLoggingDecorator>>();

                var service = new BookRepository(dbContext);
                return new BookRepositoryLoggingDecorator(service, logger);
            });
        }
    }
}