using System.Collections.Generic;
using Book.API.Queries.V1.Dtos;
using MediatR;

namespace Book.API.Queries.V1
{
    public class GetAllBooksQuery : IRequest<IEnumerable<BookDto>>
    {
    }
}