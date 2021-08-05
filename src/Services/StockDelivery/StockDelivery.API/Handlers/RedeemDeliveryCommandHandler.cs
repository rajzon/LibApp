using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using StockDelivery.API.Commands.V1;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Domain;
using StockDelivery.API.Queries.V1;

namespace StockDelivery.API.Handlers
{
    public class RedeemDeliveryCommandHandler : IRequestHandler<RedeemDeliveryCommand, RedeemDeliveryCommandResult>
    {
        private readonly IActiveDeliveryRepository _activeDeliveryRepository;
        private readonly IBookStockRepository _bookStockRepository;
        private readonly ICompletedDeliveryRepository _completedDeliveryRepository;
        private readonly ILogger<RedeemDeliveryCommandHandler> _logger;

        public RedeemDeliveryCommandHandler(IActiveDeliveryRepository activeDeliveryRepository,
            IBookStockRepository bookStockRepository, ICompletedDeliveryRepository completedDeliveryRepository,
            ILogger<RedeemDeliveryCommandHandler> logger)
        {
            _activeDeliveryRepository = activeDeliveryRepository;
            _bookStockRepository = bookStockRepository;
            _completedDeliveryRepository = completedDeliveryRepository;
            _logger = logger;
        }
        
        public async Task<RedeemDeliveryCommandResult> Handle(RedeemDeliveryCommand request, CancellationToken cancellationToken)
        {
            var deliveryToRedeem = await _activeDeliveryRepository.FindByIdAsync(request.Id);

            if (deliveryToRedeem is null)
                return new RedeemDeliveryCommandResult(false,
                    new List<string> {$"Active Delivery: {request.Id} not found"});

            // if (deliveryToRedeem.RedeemDelivery(out string error))
            //     return new RedeemDeliveryCommandResult(false, new List<string> {error});
            
            if (!_activeDeliveryRepository.RedeemDelivery(deliveryToRedeem, out string error))
                return new RedeemDeliveryCommandResult(false, new List<string> {error});

            var createdStocks = new List<BookStock>();
            foreach (var item in deliveryToRedeem.Items)
            {
                for (var i = 0; i < item.ItemsCount; i++)
                {
                    var stockToAdd = new BookStock(deliveryToRedeem, item);
                    _bookStockRepository.Add(stockToAdd);
                    createdStocks.Add(stockToAdd);
                }
            }
            
            var completedDelivery = new CompletedDelivery(deliveryToRedeem, createdStocks);
            if (_completedDeliveryRepository.Add(completedDelivery) is null)
                return new RedeemDeliveryCommandResult(false,
                    new List<string> {"Something wend wrong during adding Completed Delivery"});
            
            if (await _activeDeliveryRepository.UnitOfWork.SaveChangesAsync() < 1)
                return new RedeemDeliveryCommandResult(false, new List<string>() {"Error occured during saving to DB"});
            
            return new RedeemDeliveryCommandResult(true);
        }
    }
}