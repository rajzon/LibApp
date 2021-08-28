using System;
using System.Collections.Generic;

namespace StockDelivery.API.Queries.V1.Dtos
{
    public class ActiveDeliveryScanInfoDto
    {
        public ActiveDeliveryDto ActiveDeliveryInfo { get; init; }
        public IEnumerable<ActiveDeliveryScanItemInfoDto> Items { get; init; }
    }
    
    public class ActiveDeliveryScanItemInfoDto
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
}