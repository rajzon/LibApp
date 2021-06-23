using System.Collections.Generic;
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
    }
}