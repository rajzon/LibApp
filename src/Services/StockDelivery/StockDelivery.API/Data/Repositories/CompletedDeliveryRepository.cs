using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Data.Repositories
{
    public class CompletedDeliveryRepository : ICompletedDeliveryRepository
    {
        private readonly DeliveryStockDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public CompletedDeliveryRepository(DeliveryStockDbContext context)
        {
            _context = context;
        }
        
        
        public CompletedDelivery Add(CompletedDelivery completedDelivery)
        {
            return _context.CompletedDeliveries.Add(completedDelivery).Entity;
        }

        public async Task<IEnumerable<CompletedDelivery>> GetAllAsync()
        {
            return await _context.CompletedDeliveries.ToListAsync();
        }
    }
}