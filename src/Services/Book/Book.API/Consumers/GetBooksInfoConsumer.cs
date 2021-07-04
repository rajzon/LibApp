using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Domain;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Book.API.Consumers
{
    public class GetBooksInfoConsumer : IConsumer<GetBooksInfo>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly ILogger<CheckBooksExistanceConsumer> _logger;
        private readonly IMapper _mapper;

        public GetBooksInfoConsumer(IBookRepository bookRepository,
            IPublisherRepository publisherRepository,
            ILogger<CheckBooksExistanceConsumer> logger,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _publisherRepository = publisherRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task Consume(ConsumeContext<GetBooksInfo> context)
        {
            _logger.LogInformation("GetBooksInfoConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);
            
            var books = await _bookRepository.GetAllByIds(context.Message.BooksIds);
            var results = _mapper.Map<IEnumerable<BookInfoDto>>(books).ToList();
            

            
            foreach (var book in books)
            {
                foreach (var res in results)
                {
                    var pub = await _publisherRepository.FindByIdAsync(book.PublisherId ?? 0);
                    var img = book.Images?.FirstOrDefault(i => i.IsMain);
                    res.Publisher = pub is not null ? _mapper.Map<PublisherBusResponseDto>(pub) : null;
                    res.Image = img is not null ? _mapper.Map<ImageBusResponseDto>(img) : null;
                }
            }
            
            _logger.LogInformation("GetBooksInfoConsumer: TEST {MessageId} : {@A}",context.MessageId, results);
            
            await context.RespondAsync<BooksInfoResult>(new
            {
                Results = results
            });
        }
    }
}