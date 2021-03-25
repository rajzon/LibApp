using System.Collections.Generic;
using Book.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.API.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // builder
            //     .Property("BooksIds")
            //     .UsePropertyAccessMode(PropertyAccessMode.Field)
            //     .IsRequired(false);
            
            builder
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}