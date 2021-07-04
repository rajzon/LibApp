using MediatR;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Queries.V1
{
    public class GetActiveDeliveryQuery : IRequest<ActiveDeliveryWithItemsDto>
    {
        public int Id { get; init; }

        public GetActiveDeliveryQuery(int id)
        {
            Id = id;
        }
    }
}