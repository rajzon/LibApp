using Lend.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lend.API.Data.Configurations
{
    public class LendedBasketConfiguration : IEntityTypeConfiguration<LendedBasket>
    {
        public void Configure(EntityTypeBuilder<LendedBasket> builder)
        {
            builder.HasIndex(b => b.Email)
                .IsUnique();
        }
    }
}