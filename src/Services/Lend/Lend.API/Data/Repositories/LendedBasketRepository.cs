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
            return await _lendDbContext.LendedBaskets.Include(lb => lb.Stocks)
                .Where(lb => lb.Email == email).ToListAsync();
        }

        public async Task<IEnumerable<LendedBasket>> GetAllByStocksIds(List<int> stocksIds)
        {
            return await _lendDbContext.LendedBaskets.Include(lb => lb.Stocks)
                .Where(lb => lb.Stocks.Any(s => stocksIds.Contains(s.StockId))).ToListAsync();
        }
        
        public async Task<IEnumerable<LendedBasket>> GetAllByStockId(int stockId)
        {
            return await _lendDbContext.LendedBaskets.Include(lb => lb.Stocks)
                .Where(lb => lb.Stocks.Any(s => s.StockId == stockId)).ToListAsync();
        }

        public LendedBasket Add(LendedBasket lendedBasket)
        {
            return _lendDbContext.LendedBaskets.Add(lendedBasket).Entity;
        }
    }
}