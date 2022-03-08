using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Data.Repositories
{
    public class BookStockRepository : IBookStockRepository
    {
        private readonly DeliveryStockDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public BookStockRepository(DeliveryStockDbContext context)
        {
            _context = context;
        }

        public void Remove(BookStock stock)
        {
            _context.BookStocks.Remove(stock);
        }
        
        public void Remove(List<BookStock> stocks)
        {
            _context.BookStocks.RemoveRange(stocks);
        }
        
        public BookStock Add(BookStock stock)
        {
            return _context.BookStocks.Add(stock).Entity;
        }

        public async Task<IEnumerable<BookStock>> GetAllAsync()
        {
            return await _context.BookStocks
                .Include(s => s.BookEan13).ToListAsync();
        }

        public async Task<BookStock> GetAsync(int stockId)
        {
            return await _context.BookStocks.FirstOrDefaultAsync(s => s.Id == stockId);
        }

        public async Task<bool> IsAllExists(List<int> stocksIds)
        {
            var res = await _context.BookStocks.Where(s => stocksIds.Contains(s.Id)).ToListAsync();
            return res.Count == stocksIds.Count;
        }

        public async Task<bool> IsExist(int stockId)
        {
            return await _context.BookStocks.FirstOrDefaultAsync(s => s.Id == stockId) is not null;
        }

        
    }
}