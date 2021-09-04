using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Domain;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Book.API.Consumers
{
    public class GetBookInfoConsumer : IConsumer<GetBookInfo>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly ILogger<GetBookInfoConsumer> _logger;
        private readonly IMapper _mapper;

        public GetBookInfoConsumer(IBookRepository bookRepository,
            IPublisherRepository publisherRepository,
            ILogger<GetBookInfoConsumer> logger,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _publisherRepository = publisherRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task Consume(ConsumeContext<GetBookInfo> context)
        {
            _logger.LogInformation("GetBookInfoConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);
            var book = (await _bookRepository.GetAllByIds(new List<int>() {context.Message.BookId})).FirstOrDefault();
            if (book is null)
            {
                await context.RespondAsync<BookInfoResult>(new
                {
                    Result = (BookInfoDto) null
                });
            }
            var result = _mapper.Map<BookInfoDto>(book);
            var pub = await _publisherRepository.FindByIdAsync(book.PublisherId ?? 0);
            var img = book.Images?.FirstOrDefault(i => i.IsMain);
            result.Publisher = pub is not null ? _mapper.Map<PublisherBusResponseDto>(pub) : null;
            result.Image = img is not null ? _mapper.Map<ImageBusResponseDto>(img) : null;
            
            _logger.LogInformation("GetBookInfoConsumer: Message: {MessageId} Ended with value : {@Value}",context.MessageId, result);
            await context.RespondAsync<BookInfoResult>(new
            {
                Result = result
            });
        }
    }
}