using System;
using Microsoft.EntityFrameworkCore;
using User.Data.Configurations;
using User.Domain;
using User.Domain.Common;

namespace User.Data
{
    public class UserDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Customer> Customers { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            :base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new AddressCorrespondenceConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}