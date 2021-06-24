using System;
using System.Collections.Generic;

namespace Search.API.Contracts.Responses
{
    public class BookDeliveryResponse
    {
        public int Id { get; init; }
        
        public string Title { get; init; }
        public string Ean13 { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }


        public ImageResponseDto? Image { get; init; }
        
        public IReadOnlyCollection<CategoryResponseDto> Categories { get; init; }

        public PublisherResponseDto Publisher { get; init; }
        
        public IReadOnlyCollection<AuthorResponseDto> Authors { get; init; }


        public DateTime? PublishedDate { get; init; }
    }
    
}