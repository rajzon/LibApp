using System.Threading;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Data
{
    public class BookDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Domain.Book> Books { get; set; }
        
        public BookDbContext(DbContextOptions<BookDbContext> options)
            :base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Book>().OwnsOne<BookEan13>(book => book.Ean13);
            modelBuilder.Entity<Domain.Book>().OwnsOne<BookIsbn10>(book => book.Isbn10);
            modelBuilder.Entity<Domain.Book>().OwnsOne<BookIsbn13>(book => book.Isbn13);
            
            base.OnModelCreating(modelBuilder);
        }

        
    }
}