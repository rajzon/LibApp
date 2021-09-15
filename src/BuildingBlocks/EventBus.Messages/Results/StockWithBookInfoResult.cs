using System;
using System.Collections.Generic;

namespace EventBus.Messages.Results
{
    public class StockWithBookInfoResult
    {
        public StockWithBookInfoBusResponse StockWithBookInfo { get; set; }
    }

    public class StockWithBookInfoBusResponse
    {
        public int StockId { get; set; }
        public string Title { get; init; }
        public string Ean13 { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public DateTime PublishedDate { get; init; }
        
        public IEnumerable<CategoryBusResponseDto> Categories { get; init; }
        public IEnumerable<AuthorBusResponseDto> Authors { get; init; }
        public PublisherBusResponseDto Publisher { get; init; }
        public ImageBusResponseDto Image { get; init; }
    }
}