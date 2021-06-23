using System;
using System.Collections.Generic;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Commands.V1.Dtos
{
    public class CommandActiveDeliveryDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public bool IsAllDeliveryItemsScanned { get; init; }
        
        public DeliveryStatus DeliveryStatus { get; init; }
        public DateTime ModificationDate { get; init; }
        public DateTime CreationDate { get; init; }

        public IReadOnlyCollection<CommandActiveDeliveryItemDto> Items { get; init; }
    }
}