using Book.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Domain.Book>
    {
        public void Configure(EntityTypeBuilder<Domain.Book> builder)
        {
            builder
                .OwnsOne<BookEan13>(book => book.Ean13)
                .Property<string>("_code")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
            
            builder
                .OwnsOne<BookIsbn10>(book => book.Isbn10);
            builder
                .OwnsOne<BookIsbn13>(book => book.Isbn13);

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
                .Property<int>("_authorId")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder
                .HasOne<Author>()
                .WithMany()
                .HasForeignKey("_authorId");

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