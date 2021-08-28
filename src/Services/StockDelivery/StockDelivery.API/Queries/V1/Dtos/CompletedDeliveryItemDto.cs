using System;
using System.Collections.Generic;

namespace StockDelivery.API.Queries.V1.Dtos
{
    public class CompletedDeliveryItemDto
    {
        public int Id { get; init; }
        public int BookId { get; init; }
        public string Ean { get; init; }
        public short ItemsCount { get; init; }
        
        public IEnumerable<int> Stocks { get; init; }
        
        public DateTime ModificationDate { get; init; }
        public DateTime CreationDate { get; init; }
    }
}