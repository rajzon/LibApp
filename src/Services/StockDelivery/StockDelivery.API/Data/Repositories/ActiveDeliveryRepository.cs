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

        public void Remove(ActiveDelivery activeDelivery)
        {
            _context.ActiveDeliveries.Remove(activeDelivery);
        }

        public bool RedeemDelivery(ActiveDelivery activeDelivery, out string error)
        {
            error = string.Empty;
            if (!activeDelivery.IsRedeemOperationAllowed())
                error = "Redeem operation is not allowed";
            if (!string.IsNullOrEmpty(error))
                return false;

            _context.ActiveDeliveries.Remove(activeDelivery);
            return true;
        }

        public async Task<ActiveDelivery> FindByIdAsync(int activeDeliveryId)
        {
            var response = await _context.ActiveDeliveries.FindAsync(activeDeliveryId);

            return response;
        }

        public async Task<PagedList<ActiveDelivery>> GetAllAsync(PaginationParams paginationParams)
        {
            var response = _context.ActiveDeliveries.AsQueryable();

            return await PagedList<ActiveDelivery>.CreateAsync(response, paginationParams.CurrentPage, paginationParams.PageSize);
        }
    }
}