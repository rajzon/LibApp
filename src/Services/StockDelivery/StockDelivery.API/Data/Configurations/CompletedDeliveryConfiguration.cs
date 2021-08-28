using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Data.Configurations
{
    public class CompletedDeliveryConfiguration: IEntityTypeConfiguration<CompletedDelivery>
    {
        public void Configure(EntityTypeBuilder<CompletedDelivery> builder)
        {
            builder.Property(d => d.Name)
                .HasMaxLength(50);

            builder
                .OwnsMany<CompletedDeliveryItem>("_items", items =>
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
                    items.Property(i => i.Stocks)
                        .HasConversion(
                            v => JsonConvert.SerializeObject(v),
                            v => JsonConvert.DeserializeObject<List<int>>(v));
                })
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}