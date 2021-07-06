using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using MassTransit.Clients;
using MediatR;
using StockDelivery.API.Commands.V1;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Handlers
{
    public class EditActiveDeliveryCommandHandler : IRequestHandler<EditActiveDeliveryCommand, EditActiveDeliveryCommandResult>
    {
        private readonly IRequestClient<CheckBooksExsitance> _client;
        private readonly IActiveDeliveryRepository _activeDeliveryRepository;

        public EditActiveDeliveryCommandHandler(IActiveDeliveryRepository activeDeliveryRepository, IRequestClient<CheckBooksExsitance> client)
        {
            _activeDeliveryRepository = activeDeliveryRepository;
            _client = client;
        }
        
        public async Task<EditActiveDeliveryCommandResult> Handle(EditActiveDeliveryCommand request, CancellationToken cancellationToken)
        {
            var resultExistance = await _client.GetResponse<BooksExitanceResult>(
                new { BooksIdsWithEans = request.Items
                    .ToDictionary(key => key.BookId, value => value.BookEan) });

            if(! resultExistance.Message.IsAllExists)
                return new EditActiveDeliveryCommandResult(false, 
                    new []{ "Some of requested books not exists or they Ids do not match with corresponding Eans" });
            
            var activeDelivery = await _activeDeliveryRepository.FindByIdAsync(request.Id);

            if (activeDelivery.IsAnyDeliveryItemsScanned)
                return new EditActiveDeliveryCommandResult(false, 
                    new[] {$"You cant edit Active Delivery ID:{activeDelivery.Id} if it has scanned items!"});
            
            
            foreach (var item in request.Items)
            {
                activeDelivery.AddDeliveryItem(item.BookId, item.BookEan, item.ItemsCount);
                activeDelivery.EditDeliveryItemsCount(item.ItemId, item.ItemsCount);
            }
            activeDelivery.DeleteMissingDeliveryItems(request.Items.Select(i => i.ItemId));
            
            if(await _activeDeliveryRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 1)
                return new EditActiveDeliveryCommandResult(false,
                    new[] {$"Something went wrong during saving Active Delivery ID:{activeDelivery.Id}"});
            
            
            return new EditActiveDeliveryCommandResult(true);
        }
    }
}