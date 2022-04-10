using Book.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.Configurations
{
    public class Isbn10Configuration : IEntityTypeConfiguration<BookIsbn10>
    {
        public void Configure(EntityTypeBuilder<BookIsbn10> builder)
        {
            builder.Property(i => i.Code)
                .HasMaxLength(10);
            builder.HasIndex(e => e.Code)
                .IsUnique();
        }
    }
}