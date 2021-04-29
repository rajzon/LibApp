using Book.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Domain.Book>
    {
        public void Configure(EntityTypeBuilder<Domain.Book> builder)
        {
            builder.Property(book => book.Title)
                .HasMaxLength(200);
            builder.Property(book => book.Description)
                .HasMaxLength(20000);

            builder
                .OwnsOne<BookEan13>(book => book.Ean13, ean13 =>
                {
                    ean13.Property(e => e.Code)
                        .HasMaxLength(13);
                    ean13.HasIndex(e => e.Code)
                        .IsUnique();
                })
                .Property<string>("_code")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
            
            builder
                .OwnsOne<BookIsbn10>(book => book.Isbn10, isbn10 =>
                {
                    isbn10.Property(i => i.Code)
                        .HasMaxLength(10);
                    isbn10.HasIndex(e => e.Code)
                        .IsUnique();
                });
            
            builder
                .OwnsOne<BookIsbn13>(book => book.Isbn13, isbn13 =>
                {
                    isbn13.Property(i => i.Code)
                        .HasMaxLength(13);
                    isbn13.HasIndex(i => i.Code)
                        .IsUnique();
                });

            builder
                .Property<int?>("_languageId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired(false);

            builder
                .HasOne<Language>()
                .WithMany()
                .IsRequired(false)
                .HasForeignKey("_languageId");

            builder
                .Property<int?>("_publisherId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired(false);

            builder
                .HasOne<Publisher>()
                .WithMany()
                .IsRequired(false)
                .HasForeignKey("_publisherId");
            
        }
    }
}