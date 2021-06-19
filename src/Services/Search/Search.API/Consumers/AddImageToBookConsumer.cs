using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Elasticsearch.Net;
using EventBus.Messages.Commands;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Search.API.Domain;

namespace Search.API.Consumers
{
    public class AddImageToBookConsumer : IConsumer<AddImageToBook>
    {
        private readonly ILogger<AddImageToBookConsumer> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AddImageToBookConsumer(ILogger<AddImageToBookConsumer> logger,
            IElasticClient elasticClient, IMapper mapper,
            IConfiguration configuration)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _mapper = mapper;
            _configuration = configuration;
        }
        
        public async Task Consume(ConsumeContext<AddImageToBook> context)
        {
            _logger.LogInformation("AddImageToBookConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);

            var updateResult = await _elasticClient.UpdateByQueryAsync<Book>(u =>
                u.Index(_configuration["elasticsearch:bookIndexName"])
                    .Query(q =>
                        q.Term(b => b.Id, context.Message.BookId))
                    .Script(s =>
                        s.Source("if (ctx._source.images == null) { ctx._source.images = new ArrayList(); } else { ctx._source.images.add(params.elem); }")
                            .Params(p => p.Add("elem", _mapper.Map<Image>(context.Message.Image)))).Conflicts(Conflicts.Proceed));


            if (! updateResult.IsValid)
                 _logger.LogError("AddImageToBookConsumer: Message {MessageId} failed to insert data to Elasticsearch", context.MessageId);
            else
                 _logger.LogInformation("AddImageToBookConsumer: Message {MessageId} successfully inserted data to Elasticsearch", context.MessageId);

        }
        
     
    }
}