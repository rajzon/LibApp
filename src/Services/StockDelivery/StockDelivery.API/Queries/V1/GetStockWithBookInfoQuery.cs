using MediatR;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Queries.V1
{
    public class GetStockWithBookInfoQuery : IRequest<StockWithBookInfoDto>
    {
        public string BookEan { get; set; }
    }
}