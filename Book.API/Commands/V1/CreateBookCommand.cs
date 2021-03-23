using Book.API.Responses.V1;
using MediatR;

namespace Book.API.Commands.V1
{
    public class CreateBookCommand : IRequest<BookResponse>
    {
        public string Title { get; init; }
        
    }
}