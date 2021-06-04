using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Search.API.Application.Services;
using Search.API.Application.Services.Common;
using Search.API.Commands;
using Search.API.Domain;
using Search.API.Extensions;
using FieldType = Search.API.Application.Services.Common.FieldType;

namespace Search.API.Infrastructure.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly IElasticClient _client;
        private readonly ILogger<IBookRepository> _logger;
        private Dictionary<string, FieldType> _bookManagementSortAllowedValues => BookRepositorySettings.BOOK_MANAGEMENT_SORT_ALLOWED_VALUES;
        
        public BookRepository(IElasticClient client, ILogger<IBookRepository> logger)
        {
            _client = client;
            _logger = logger;
        }
        
        public async Task<ISearchResponse<Book>> SearchAsync(SearchBookCommand command)
        {
            _logger.LogInformation("BookRepository: SearchAsync - Method started with values {@MethodValues}", command);

            if (command.FromPage.Equals(0))
                command.FromPage = 1;
            if (command.PageSize.Equals(0))
                command.PageSize = BookRepositorySettings.DEFAULT_PAGINATION_PAGE_SIZE;
            

            var result = await _client.SearchAsync<Book>(s => s
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
                                .Terms(t => t
                                    .Field(f => f
                                        .Authors.Select(a => a.Name).First().FullName.Suffix("keyword"))
                                    .Terms(command.Authors)
                                ), fi => fi
                                .Terms(t => t
                                    .Field(f => f.Categories.Select(c => c.Name).First().Suffix("keyword"))
                                    .Terms(command.Categories)
                            ), fi => fi
                                .Terms(t => t
                                    .Field(f => f.Language.Name.Suffix("keyword"))
                                    .Terms(command.Languages)),
                            fi => fi
                                .Terms(m => m
                                    .Field(f => f.Publisher.Name.Suffix("keyword"))
                                    .Terms(command.Publishers)),
                            fi => fi
                                .Terms(t => t
                                    .Field(f => f.Visibility)
                                    .Terms(command.Visibility))
                        )
                    )).Sort(ss => ss.Custom(command.SortBy, _bookManagementSortAllowedValues)
                ).Aggregations(ag => ag
                    .Terms("categories", t => t
                        .Field(f => f.Categories.First().Name.Suffix("keyword")))
                    .Terms("visibility", t => t
                        .Field(f => f.Visibility))
                    .Terms("authors", t => t
                        .Field(f => f.Authors.First().Name.FullName.Suffix("keyword")))
                    .Terms("languages", t => t
                        .Field(f => f.Language.Name.Suffix("keyword")))
                    .Terms("publishers", t => t
                        .Field(f => f.Publisher.Name.Suffix("keyword"))))
                .From((command.FromPage - 1) * command.PageSize)
                .Size(command.PageSize)
            );

            if (! result.IsValid)
                _logger.LogError("BookRepository: SearchAsync error occured during analyzing query by Elasticsearch, {@MethodValues}", command);
            
            
            _logger.LogInformation("BookRepository: SearchAsync successfully requested data from Elasticsearch");
            return result;
        }
    }
}