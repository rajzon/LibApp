using Book.API.Responses;
using MediatR;

namespace Book.API.Commands
{
    public class CreateBookCommand : IRequest<BookResponse>
    {
        public string Title { get; init; }
        
    }
}