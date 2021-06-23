using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Data.Configurations
{
    public class ActiveDeliveryConfiguration : IEntityTypeConfiguration<ActiveDelivery>
    {
        public void Configure(EntityTypeBuilder<ActiveDelivery> builder)
        {
            builder.Property(d => d.Name)
                .HasMaxLength(50);
            
            builder
                .OwnsMany<ActiveDeliveryItem>("_items", items =>
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