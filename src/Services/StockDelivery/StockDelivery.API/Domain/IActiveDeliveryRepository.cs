using System.Collections.Generic;
using System.Threading.Tasks;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public interface IActiveDeliveryRepository : IRepository<ActiveDelivery>
    {
        ActiveDelivery Add(ActiveDelivery activeDelivery);
        void Remove(ActiveDelivery activeDelivery);
        bool RedeemDelivery(ActiveDelivery activeDelivery, out string error);
        Task<ActiveDelivery> FindByIdAsync(int activeDeliveryId);
        Task<PagedList<ActiveDelivery>> GetAllAsync(PaginationParams paginationParams);
    }
}