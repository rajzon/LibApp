using Book.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.Configurations
{
    public class Ean13Configuration : IEntityTypeConfiguration<BookEan13>
    {
        public void Configure(EntityTypeBuilder<BookEan13> builder)
        {
            builder.Property(e => e.Code)
                .HasMaxLength(13);
            builder.HasIndex(e => e.Code)
                .IsUnique();
        }
    }
}