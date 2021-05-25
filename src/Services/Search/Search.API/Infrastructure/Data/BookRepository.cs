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

namespace Search.API.Infrastructure.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly IElasticClient _client;
        private readonly ILogger<IBookRepository> _logger;
        private string[] _bookManagementSortAllowedValues => BookRepositorySettings.BOOK_MANAGEMENT_SORT_ALLOWED_VALUES;

        public BookRepository(IElasticClient client, ILogger<IBookRepository> logger)
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
                    ))
                .Sort(ss =>
                    {
                        var sortValue = command.SortBy?.Split(':');
                        string selectedSortingField = string.Empty;

                        for (int i = 0; i < sortValue?.Length; i++)
                        {

                            if (i == 0)
                            {
                                selectedSortingField = _bookManagementSortAllowedValues.FirstOrDefault(b => b
                                    .Contains(sortValue[0]));
                            }

                            if (i == 1)
                            {
                                if (selectedSortingField != null && selectedSortingField.Equals("modificationDate"))
                                {
                                    ss = sortValue[1].Contains("desc")
                                        ? ss.Descending(selectedSortingField)
                                        : ss.Ascending(selectedSortingField);
                                }
                                else if (!string.IsNullOrEmpty(selectedSortingField))
                                {
                                    ss = sortValue[1].Contains("desc")
                                        ? ss.Descending(selectedSortingField + ".keyword")
                                        : ss.Ascending(selectedSortingField + ".keyword");
                                }
                            }

                        }

                        return ss;
                    }
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


        }
    }
}