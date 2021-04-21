using System;
using MediatR;

namespace Book.API.Commands.V1
{
    public class CreateBookManualCommand : IRequest<CreateBookCommandResult>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public ushort? PageCount { get; init; }
        public bool Visibility { get; init; }
        
        public int? AuthorId { get; init; }
        public int? LanguageId { get; init; }
        public int? PublisherId { get; init; }
        public int[] CategoriesIds { get; init; }
        
        
        public DateTime? PublishedDate { get; init; }
    }
    
}