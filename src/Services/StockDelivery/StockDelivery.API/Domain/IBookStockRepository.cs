using System.Collections.Generic;
using System.Threading.Tasks;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public interface IBookStockRepository : IRepository<CompletedDelivery>
    {
        BookStock Add(BookStock stock);
        Task<IEnumerable<BookStock>> GetAllAsync();
        Task<BookStock> GetAsync(int stockId);
    }
}