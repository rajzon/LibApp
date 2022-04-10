using Book.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.Configurations
{
    public class Isbn13Configuration : IEntityTypeConfiguration<BookIsbn13>
    {
        public void Configure(EntityTypeBuilder<BookIsbn13> builder)
        {
            builder.Property(i => i.Code)
                .HasMaxLength(13);
            builder.HasIndex(i => i.Code)
                .IsUnique();
        }
    }
}