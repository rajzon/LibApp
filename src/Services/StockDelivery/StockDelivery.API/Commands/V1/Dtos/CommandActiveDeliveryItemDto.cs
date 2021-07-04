using System;

namespace StockDelivery.API.Commands.V1.Dtos
{
    public class CommandActiveDeliveryItemDto
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
    }
}