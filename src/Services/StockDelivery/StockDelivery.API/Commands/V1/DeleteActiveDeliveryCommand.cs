using MediatR;
using StockDelivery.API.Controllers.V1;

namespace StockDelivery.API.Commands.V1
{
    public class DeleteActiveDeliveryCommand : IRequest<DeleteActiveDeliveryCommandResult>
    {
        public int DeliveryId { get; init; }
        public string CancellationReason { get; init; }
    }
}