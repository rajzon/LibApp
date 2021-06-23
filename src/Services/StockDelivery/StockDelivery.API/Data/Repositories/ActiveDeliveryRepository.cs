using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Data.Repositories
{
    public class ActiveDeliveryRepository : IActiveDeliveryRepository
    {
        private readonly DeliveryStockDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ActiveDeliveryRepository(DeliveryStockDbContext context)
        {
            _context = context;
        }
        
        
        public ActiveDelivery Add(ActiveDelivery activeDelivery)
        {
            var response = _context.ActiveDeliveries.Add(activeDelivery).Entity;

            return response;
        }

        public async Task<PagedList<ActiveDelivery>> GetAllAsync(PaginationParams paginationParams)
        {
            var response = _context.ActiveDeliveries.AsQueryable();

            return await PagedList<ActiveDelivery>.CreateAsync(response, paginationParams.CurrentPage, paginationParams.PageSize);
        }
    }
}