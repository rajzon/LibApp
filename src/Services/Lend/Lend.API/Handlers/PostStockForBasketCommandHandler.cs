using System.Threading;
using System.Threading.Tasks;
using Lend.API.Controllers.V1;
using MediatR;

namespace Lend.API.Handlers
{
    public class PostStockForBasketCommandHandler : IRequestHandler<PostStockForBasketCommand, PostBasketCommandResult>
    {
        public Task<PostBasketCommandResult> Handle(PostStockForBasketCommand request, CancellationToken cancellationToken)
        {
            //TODO check if requested stock exists, return NotFound
            //TODO check if stockId is already lended to other customer, return Conflict
            //TODO do not allow same StockId to be placed in basket, return Forbid
            
            //TODO FOR Existing Stocks: check if passed return date is not exceeding max allowed by organization(call strategy), or Call strategy
            //TODO... during creation of Basket and then fire STRATEGY
            //TODO FOR NEW Stocks: set default return date to max allowed by organization( call strategy/rule)
            
            //TODO wywołać IsBasketMatchStrategy(Basket basket) oraz Task<(bool, StrategyError)> IsCustomerMatchStrategy(CustomerBasket basket) !!!
            throw new System.NotImplementedException();
        }
    }
}