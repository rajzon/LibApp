using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using StockDelivery.API.Domain;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Queries.V1;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Handlers
{
    public class GetAllActiveDeliveriesQueryHandler : IRequestHandler<GetAllActiveDeliveriesQuery, PaginatedResult<ActiveDeliveryDto>>
    {
        private readonly IActiveDeliveryRepository _activeDeliveryRepository;
        private readonly IMapper _mapper;

        public GetAllActiveDeliveriesQueryHandler(IActiveDeliveryRepository activeDeliveryRepository,
            IMapper mapper)
        {
            _activeDeliveryRepository = activeDeliveryRepository;
            _mapper = mapper;
        }
        
        public async Task<PaginatedResult<ActiveDeliveryDto>> Handle(GetAllActiveDeliveriesQuery request, CancellationToken cancellationToken)
        {

            var paginatedDeliveries = await _activeDeliveryRepository.GetAllAsync(
                new PaginationParams(request.CurrentPage, request.PageSize));
            
            var result = _mapper.Map<IReadOnlyCollection<ActiveDeliveryDto>>(paginatedDeliveries);

            return new PaginatedResult<ActiveDeliveryDto>(result, paginatedDeliveries.CurrentPage, paginatedDeliveries.PageSize,
                paginatedDeliveries.TotalCount);
        }
    }

    public class PaginatedResult<T> 
        where T: class 
    {
        public short CurrentPage { get; init; }
        public short PageSize { get; init; }
        public int Total { get; init; }
        
        public IReadOnlyCollection<T> Result { get; init; }

        public PaginatedResult(IReadOnlyCollection<T> result, short currentPage, short pageSize, int total)
        {
            Result = result;
            
            Total = total;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
        
    }
}