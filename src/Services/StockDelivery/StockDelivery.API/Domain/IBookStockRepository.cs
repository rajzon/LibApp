using System.Collections.Generic;
using System.Threading.Tasks;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public interface IBookStockRepository : IRepository<CompletedDelivery>
    {
        void Remove(BookStock stock);
        void Remove(List<BookStock> stocks);
        BookStock Add(BookStock stock);
        Task<IEnumerable<BookStock>> GetAllAsync();
        Task<BookStock> GetAsync(int stockId);
        Task<bool> IsAllExists(List<int> stocksIds);
        Task<bool> IsExist(int stockId);
    }
}