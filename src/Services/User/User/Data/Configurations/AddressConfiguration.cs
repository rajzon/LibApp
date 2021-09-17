using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain;

namespace User.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
                .OwnsOne<PostCode>(address => address.PostCode, postCode =>
                {
                    postCode.Property(i => i.Code)
                        .HasMaxLength(10);
                    postCode.HasIndex(e => e.Code)
                        .IsUnique();
                });
        }
    }
}