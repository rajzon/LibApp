using System.Collections.Generic;
using System.Threading.Tasks;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public interface ICompletedDeliveryRepository : IRepository<CompletedDelivery>
    {
        CompletedDelivery Add(CompletedDelivery completedDelivery);
        Task<IEnumerable<CompletedDelivery>> GetAllAsync();
    }
}