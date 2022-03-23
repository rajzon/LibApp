using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using Search.API.Commands;
using Search.API.Commands.V1;
using Search.API.Domain;
using Search.API.Infrastructure.Data;

namespace Search.API.Application.Services
{
    public interface ISearchRepository : Common.IRepository<Book>
    {
        Task<ISearchResponse<Book>>SearchAsync(SearchBookCommand command);
        Task<ISearchResponse<Book>>SearchByEanAsync(SearchBookByEanCommand command);
        Task<ISearchResponse<Book>> SuggestAsync(SuggestBookCommand command);
        Task<ISearchResponse<Customer>> SuggestCustomerAsync(SuggestCustomerCommand command);
        Task<ISearchResponse<Customer>> SearchCustomersByEmail(SearchCustomerCommand command);
    }
}