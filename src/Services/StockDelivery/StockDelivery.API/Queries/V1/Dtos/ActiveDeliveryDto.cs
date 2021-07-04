using System;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Queries.V1.Dtos
{
    public class ActiveDeliveryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short BooksCount { get; set; }
        public short ItemsCount { get; set; }
        public bool IsAllDeliveryItemsScanned { get; set; }
        public bool IsAnyDeliveryItemsScanned { get; set; }
        
        public DeliveryStatus DeliveryStatus { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}