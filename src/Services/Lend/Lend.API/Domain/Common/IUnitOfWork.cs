using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lend.API.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}