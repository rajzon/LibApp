using Book.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.Configurations
{
    public class AuthorConfiguration: IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.OwnsOne<AuthorName>(a => a.Name, name =>
            {
                name.Property(n => n.FirstName)
                    .HasMaxLength(40);
                name.Property(n => n.LastName)
                    .HasMaxLength(40);
                name.Property(n => n.FullName)
                    .HasMaxLength(81);
            });
        }
    }
}