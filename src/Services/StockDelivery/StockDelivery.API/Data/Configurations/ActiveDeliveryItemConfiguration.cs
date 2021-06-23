using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Data.Configurations
{
    public class ActiveDeliveryItemConfiguration : IEntityTypeConfiguration<ActiveDeliveryItem>
    {
        public void Configure(EntityTypeBuilder<ActiveDeliveryItem> builder)
        {
            builder
                .OwnsOne<BookEan13>(book => book.BookEan, ean13 =>
                {
                    ean13.Property(e => e.Code)
                        .HasMaxLength(13);
                    ean13.HasIndex(e => e.Code)
                        .IsUnique();
                })
                .Property<string>("_code")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}