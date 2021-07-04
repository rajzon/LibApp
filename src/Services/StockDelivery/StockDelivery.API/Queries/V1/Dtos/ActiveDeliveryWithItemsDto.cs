using System;
using System.Collections.Generic;
using StockDelivery.API.Controllers.V1;

namespace StockDelivery.API.Queries.V1.Dtos
{
    public class ActiveDeliveryWithItemsDto
    {
        public ActiveDeliveryDto ActiveDeliveryInfo { get; init; }
        public IEnumerable<ActiveDeliveryItemDto> Items { get; init; }
    }
    
    public class ActiveDeliveryItemDto
    {
        public int Id { get; init; }
        
        public int BookId { get; init;  }
        public string BookEan { get; init; }
        public short ItemsCount { get; init; }
        
        public short ScannedCount { get; init; }
        public bool IsScanned { get; init; }
        public bool IsAllScanned { get; init; }
        
        public DateTime ModificationDate { get; init; }
        public DateTime CreationDate { get; init; }
        
        public ActiveDeliveryItemDescDto ItemDescription { get; set; }
    }
    
    public class ActiveDeliveryItemDescDto
    {
        public string Title { get;  init; }
        public string Ean13 { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public ushort? PageCount { get; init; }
        
        public IEnumerable<CategoryResponseDto> Categories { get; init; }
        public IEnumerable<AuthorResponseDto> Authors { get; init; }
        public PublisherResponseDto Publisher { get; init; }
        public ImageResponseDto Image { get; init; }
    }
    
    public record CategoryResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
    
    
    public record AuthorResponseDto
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string FullName { get; init; }
    }
    
    
    public record ImageResponseDto
    {
        public string Url { get; init; }
        public bool IsMain { get; init; }
    }
    
    public record PublisherResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}