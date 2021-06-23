using System.Collections.Generic;
using MediatR;
using StockDelivery.API.Commands.V1.Dtos;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Handlers;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Queries.V1
{
    public class GetAllActiveDeliveriesQuery : IRequest<PaginatedResult<ActiveDeliveryDto>>
    {
        public short CurrentPage { get; init; }
        public short PageSize { get; init; }

        public GetAllActiveDeliveriesQuery(short currentPage, short pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}