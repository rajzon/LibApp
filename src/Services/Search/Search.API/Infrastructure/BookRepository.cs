using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nest;
using Search.API.Application.Services;
using Search.API.Commands;
using Search.API.Domain;

namespace Search.API.Infrastructure
{
    public class BookRepository : IBookRepository
    {
        private readonly IElasticClient _client;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(IElasticClient client, ILogger<BookRepository> logger)
        {
            _client = client;
            _logger = logger;
        }
        
        public async Task<ISearchResponse<Book>> SearchAsync(SearchBookCommand command)
        {
            //TODO: later on add more logs
            _logger.LogInformation("TODO: BookRepository: Search - Started");
            
           return await _client.SearchAsync<Book>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Must(mu => mu
                            .MultiMatch(m => m
                                .Type(TextQueryType.MostFields)
                                .Fields(fs => fs
                                    .Field(f => f.Ean13, boost: 5)
                                    .Field(f => f.Title, boost: 2)
                                    .Field(f => f.Authors.Select(a => a.Name).First().FullName))
                                .Operator(Operator.Or)
                                .Query(command.SearchTerm))
                        )
                        .Filter(fi => fi
                            .DateRange(r => r
                                .Field(f => f.ModificationDate)
                                .GreaterThanOrEquals(command.ModificationDateFrom)
                                .LessThanOrEquals(command.ModificationDateTo)), fi => fi
                            .Match(t => t
                                .Field(f => f
                                    .Authors.Select(a => a.Name).First().FullName)
                                .Query(command.Authors)
                            ), fi => fi
                            .Match(t => t
                            .Field(f => f.Categories.Select(c => c.Name).First())
                                .Query(command.Categories)), fi => fi
                            .Match(t => t
                                .Field(f => f.Language.Name)
                                .Query(command.Languages)), 
                            fi => fi
                                .Match(m => m
                                .Field(f => f.Publisher.Name)
                                .Query(command.Publishers)), 
                            fi => fi
                                .Term(t => t
                                .Field(f => f.Visibility)
                                .Value(command.Visibility))
                        )
                    )));
        }
    }
}