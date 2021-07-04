using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using MediatR;
using StockDelivery.API.Commands.V1;
using StockDelivery.API.Commands.V1.Dtos;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Handlers
{
    public class CreateActiveDeliveryCommandHandler : IRequestHandler<CreateActiveDeliveryCommand ,CreateActiveDeliveryCommandResult>
    {
        private readonly IRequestClient<CheckBooksExsitance> _client;
        private readonly IActiveDeliveryRepository _activeDeliveryRepository;
        private readonly IMapper _mapper;

        public CreateActiveDeliveryCommandHandler(IRequestClient<CheckBooksExsitance> client,
            IActiveDeliveryRepository activeDeliveryRepository,
            IMapper mapper)
        {
            _client = client;
            _activeDeliveryRepository = activeDeliveryRepository;
            _mapper = mapper;
        }
        
        public async Task<CreateActiveDeliveryCommandResult> Handle(CreateActiveDeliveryCommand request, CancellationToken cancellationToken)
        {
            var resultExistance = await _client.GetResponse<BooksExitanceResult>(
                new { BooksIdsWithEans = request.ItemsInfo
                .ToDictionary(key => key.BookId, value => value.BookEan) });

            if(! resultExistance.Message.IsAllExists)
                return new CreateActiveDeliveryCommandResult(false, 
                    new []{ "Some of requested books not exists or they Ids do not match with corresponding Eans" });

            var delivery = new ActiveDelivery(request.Name);
            AddDeliveryItems(delivery, request);

            var deliveryResult = _activeDeliveryRepository.Add(delivery);

            if (await _activeDeliveryRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 1)
                return new CreateActiveDeliveryCommandResult(false, new []{ "Something went wrong during saving Active Delivery to Db" });

            var result = _mapper.Map<CommandActiveDeliveryDto>(deliveryResult);
            
            return new CreateActiveDeliveryCommandResult(true, result);
        }

        public void AddDeliveryItems(ActiveDelivery delivery, CreateActiveDeliveryCommand command)
        {
            if (delivery is null || command is null)
                return;

            foreach (var itemInfo in command.ItemsInfo)
            {
                delivery.AddDeliveryItem(itemInfo.BookId, itemInfo.BookEan, itemInfo.ItemsCount);
            }
        }
    }
}