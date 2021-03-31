using System;
using System.Collections.Generic;
using Book.API.Queries.V1.Dtos;
using MediatR;

namespace Book.API.Commands.V1
{
    public class CreateBookCommand : IRequest<CreateBookCommandResult>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public ushort? PageCount { get; init; }
        public bool Visibility { get; init; }

        public string LanguageName { get; init; }
        public AuthorNameDto Author { get; init; }
        public string PublisherName { get; set; }
        public IEnumerable<string> CategoriesNames { get; init; }
        
        
        public DateTime? PublishedDate { get; init; }
        
        
    }

    public record AuthorNameDto
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}