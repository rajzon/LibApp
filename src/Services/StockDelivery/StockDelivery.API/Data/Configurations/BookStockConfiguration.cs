using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Data.Configurations
{
    public class BookStockConfiguration : IEntityTypeConfiguration<BookStock>
    {
        public void Configure(EntityTypeBuilder<BookStock> builder)
        {

            builder.OwnsOne<BookEan13ForStock>(book => book.BookEan13, ean13 =>
                {
                    ean13.Property(e => e.Code)
                        .HasMaxLength(13);
                })
                .Property<string>("_code")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}