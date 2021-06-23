using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Data.Configurations
{
    public class ActiveDeliveryConfiguration : IEntityTypeConfiguration<ActiveDelivery>
    {
        public void Configure(EntityTypeBuilder<ActiveDelivery> builder)
        {
            builder.Property(d => d.Name)
                .HasMaxLength(50);
        }
    }
}