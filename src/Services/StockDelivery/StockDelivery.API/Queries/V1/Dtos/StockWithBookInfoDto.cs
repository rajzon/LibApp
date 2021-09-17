using System;
using System.Collections.Generic;

namespace StockDelivery.API.Queries.V1.Dtos
{
    public class StockWithBookInfoDto
    {
        public int StockId { get; set; }
        public string Title { get;  init; }
        public string Ean13 { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public DateTime PublishedDate { get; init; }
        
        public IEnumerable<CategoryResponseDto> Categories { get; init; }
        public IEnumerable<AuthorResponseDto> Authors { get; init; }
        public PublisherResponseDto Publisher { get; init; }
        public ImageResponseDto Image { get; init; }
    }
}