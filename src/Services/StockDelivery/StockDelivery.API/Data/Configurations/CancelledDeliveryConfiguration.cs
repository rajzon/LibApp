using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Data.Configurations
{
    public class CancelledDeliveryConfiguration : IEntityTypeConfiguration<CancelledDelivery>
    {
        public void Configure(EntityTypeBuilder<CancelledDelivery> builder)
        {
            builder.Property(d => d.Name)
                .HasMaxLength(50);
            
            builder
                .OwnsMany<CancelledDeliveryItem>("_items", items =>
                {
                    items.OwnsOne<BookEan13>(book => book.BookEan, ean13 =>
                        {
                            ean13.Property(e => e.Code)
                                .HasMaxLength(13);
                            ean13.HasIndex(e => e.Code)
                                .IsUnique();
                        })
                        .Property<string>("_code")
                        .UsePropertyAccessMode(PropertyAccessMode.Field);
                })
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}