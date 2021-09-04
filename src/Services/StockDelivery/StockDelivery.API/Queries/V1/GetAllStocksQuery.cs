using System.Collections.Generic;
using MediatR;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Queries.V1
{
    public class GetAllStocksQuery : IRequest<IEnumerable<StockDto>>
    {
    }
}