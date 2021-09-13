using System.Collections.Generic;
using System.Threading.Tasks;
using Lend.API.Domain.Common;

namespace Lend.API.Domain.Repositories
{
    public interface ILendedBasketRepository : IRepository<LendedBasket>
    {
        Task<IEnumerable<LendedBasket>> GetAllByCustomerEmail(string email);
    }
}