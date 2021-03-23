using Microsoft.EntityFrameworkCore;

namespace Book.API.Data
{
    public class BookDbContext : DbContext
    {
        public DbSet<Domain.Book> Books { get; set; }
        
        public BookDbContext(DbContextOptions<BookDbContext> options)
            :base(options) { }

    }
}