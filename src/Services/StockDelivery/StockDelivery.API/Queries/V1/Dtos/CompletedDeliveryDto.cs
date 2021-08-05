using System;
using System.Collections.Generic;
using StockDelivery.API.Controllers.V1;

namespace StockDelivery.API.Queries.V1.Dtos
{
    public class CompletedDeliveryDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        
        public int ActiveDeliveryId { get; init; }
        public DateTime ModificationDate { get; init; }
        public DateTime CreationDate { get; init; }
        
        public IEnumerable<CompletedDeliveryItemDto> Items { get; init; }
    }
}