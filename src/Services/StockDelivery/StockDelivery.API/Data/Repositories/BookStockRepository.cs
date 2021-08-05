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
        
        public BookStock Add(BookStock stock)
        {
            return _context.BookStocks.Add(stock).Entity;
        }

        public async Task<IEnumerable<BookStock>> GetAllAsync()
        {
            return await _context.BookStocks.ToListAsync();
        }
    }
}