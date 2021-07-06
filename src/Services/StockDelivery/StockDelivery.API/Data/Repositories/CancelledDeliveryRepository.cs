using StockDelivery.API.Domain;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Data.Repositories
{
    public class CancelledDeliveryRepository : ICancelledDeliveryRepository
    {
        private readonly DeliveryStockDbContext _context;

        public IUnitOfWork UnitOfWork => _context;
        
        public CancelledDeliveryRepository(DeliveryStockDbContext context)
        {
            _context = context;
        }
        
        public CancelledDelivery Add(CancelledDelivery cancelledDelivery)
        {
            return _context.CancelledDeliveries.Add(cancelledDelivery).Entity;
        }
        
        public void Remove(CancelledDelivery cancelledDelivery)
        {
            _context.CancelledDeliveries.Remove(cancelledDelivery);
        }
    }
}