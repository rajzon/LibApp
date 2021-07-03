using System.Collections.Generic;

namespace EventBus.Messages.Results
{
    public class BooksInfoResult
    {
        public IEnumerable<BookInfoDto> Results { get; init; }
    }

    public class BookInfoDto
    {
        public string Title { get;  init; }
        public string Ean13 { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public ushort? PageCount { get; init; }
        
        public IReadOnlyCollection<CategoryBusResponseDto> Categories { get; set; }
        public IReadOnlyCollection<AuthorBusResponseDto> Authors { get; set; }
        public PublisherBusResponseDto Publisher { get; set; }
        public ImageBusResponseDto Image { get; set; }
    }

    public record CategoryBusResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
    
    public record ImageBusResponseDto
    {
        public string Url { get; init; }
        public bool IsMain { get; init; }
    }
    
    public record PublisherBusResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
    
    public record AuthorBusResponseDto
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string FullName { get; init; }
    }
}