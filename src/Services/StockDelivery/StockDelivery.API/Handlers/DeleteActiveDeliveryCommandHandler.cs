using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StockDelivery.API.Commands.V1;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Handlers
{
    public class DeleteActiveDeliveryCommandHandler : IRequestHandler<DeleteActiveDeliveryCommand, DeleteActiveDeliveryCommandResult>
    {
        private readonly IActiveDeliveryRepository _activeDeliveryRepository;

        public DeleteActiveDeliveryCommandHandler(IActiveDeliveryRepository activeDeliveryRepository)
        {
            _activeDeliveryRepository = activeDeliveryRepository;
        }
        
        public async Task<DeleteActiveDeliveryCommandResult> Handle(DeleteActiveDeliveryCommand request, CancellationToken cancellationToken)
        {
            var activeDelivery = await _activeDeliveryRepository.FindByIdAsync(request.DeliveryId);

            if (activeDelivery is null)
                return new DeleteActiveDeliveryCommandResult(false,
                    new[] {$"Could not find Delivery with id {request.DeliveryId}"});
            

            if (activeDelivery.IsAnyDeliveryItemsScanned)
                return new DeleteActiveDeliveryCommandResult(false, new[] {"At least 1 delivery item is scanned"});
                
            
            _activeDeliveryRepository.Remove(activeDelivery);
            if (await _activeDeliveryRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 1)
                return new DeleteActiveDeliveryCommandResult(false, new[] {"Something went wrong during saving Db"});
            
            
            return new DeleteActiveDeliveryCommandResult(true);
        }
    }
}