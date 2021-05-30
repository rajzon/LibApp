using System;
using System.Collections.Generic;
using EventBus.Messages.Events;

namespace EventBus.Messages.Commands
{
    public class CreateBook : IntegrationEvent
    {
        public int Id { get; init; }

        public string Title { get; init; }
        public string Description { get; init; }
        public string Ean13 { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public ushort? PageCount { get; init; }
        public bool Visibility { get; init; }

        
        public IReadOnlyCollection<ImageDto> Images { get; init; }
        
        public IReadOnlyCollection<CategoryDto> Categories { get; init; }
        
        public LanguageDto Language { get; set; }
        
        public PublisherDto Publisher { get; set; }
        
        public IReadOnlyCollection<AuthorDto> Authors { get; set; }


        public DateTime? PublishedDate { get; init; }
        public DateTime ModificationDate { get; init; }
        public DateTime CreationDate { get; init; }
    }

    public record LanguageDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
    
    public record PublisherDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }

    public record CategoryDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }

    public record AuthorDto
    {
        public int Id { get; init; }
        public AuthorNameDto Name { get; init; }
    }

    public record AuthorNameDto
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string FullName { get; init; }
    }
}