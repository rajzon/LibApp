using System;
using MediatR;

namespace Book.API.Commands.V1
{
    public class CreateBookCommand : IRequest<CreateBookCommandResult>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public string Ean13 { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public ushort? PageCount { get; init; }
        public bool Visibility { get; init; }
        public DateTime PublishedDate { get; init; }
        
        
    }
}