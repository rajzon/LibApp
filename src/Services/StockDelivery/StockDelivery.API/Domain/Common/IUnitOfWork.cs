using System;
using System.Threading;
using System.Threading.Tasks;

namespace StockDelivery.API.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> SaveChangesAndDoEventsAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}