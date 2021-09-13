using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lend.API.Domain;
using Lend.API.Domain.Common;
using Lend.API.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lend.API.Data.Repositories
{
    public class LendedBasketRepository : ILendedBasketRepository
    {
        private LendDbContext _lendDbContext;
        
        
        public IUnitOfWork UnitOfWork => _lendDbContext;
        

        public LendedBasketRepository(LendDbContext lendDbContext)
        {
            _lendDbContext = lendDbContext;
        }

        public async Task<IEnumerable<LendedBasket>> GetAllByCustomerEmail(string email)
        {
            return await _lendDbContext.LendedBaskets.Where(lb => lb.Email == email).ToListAsync();
        }
    }
}