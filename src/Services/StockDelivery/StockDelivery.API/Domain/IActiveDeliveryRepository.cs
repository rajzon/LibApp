using System.Collections.Generic;
using System.Threading.Tasks;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public interface IActiveDeliveryRepository : IRepository<ActiveDelivery>
    {
        ActiveDelivery Add(ActiveDelivery activeDelivery);
        Task<PagedList<ActiveDelivery>> GetAllAsync(PaginationParams paginationParams);
    }
}