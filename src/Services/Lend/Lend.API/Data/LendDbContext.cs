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

        public LendDbContext(DbContextOptions<LendDbContext> options)
            :base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SimpleIntRule>().HasData(
                new SimpleIntRule("MaxLendDays", "Maximum allowed number of days to lend book", 30,
                    typeof(MaxDaysForLendBookStrategy)),
                new SimpleIntRule("MaxPossibleBooksToLend", "Maximum books to be borrowed by user in particular point of time", 3, typeof(MaxPossibleBooksToLendStrategy)));
            
            base.OnModelCreating(modelBuilder);
        }
    }
}