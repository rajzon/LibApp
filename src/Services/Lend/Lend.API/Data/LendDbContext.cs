using Lend.API.Data.Configurations;
using Lend.API.Domain;
using Lend.API.Domain.Common;
using Lend.API.Domain.Strategies;
using Microsoft.EntityFrameworkCore;

namespace Lend.API.Data
{
    public class LendDbContext : DbContext, IUnitOfWork
    {
        public DbSet<SimpleIntRule> SimpleIntRules { get; set; }
        public DbSet<SimpleBooleanRule> SimpleBooleanRules { get; set; }
        public DbSet<LendedBasket> LendedBaskets { get; set; }

        public LendDbContext(DbContextOptions<LendDbContext> options)
            :base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LendedBasketConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}