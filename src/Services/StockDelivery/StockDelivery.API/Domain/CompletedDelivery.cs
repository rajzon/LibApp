using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public class CompletedDelivery : Entity, IDelivery, IAggregateRoot
    {
        public string Name { get; private set; }
        
        public DeliveryStatus DeliveryStatus { get; }
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        public IReadOnlyCollection<IDeliveryItem> Items { get; private set; }

        public CompletedDelivery()
        {
            DeliveryStatus = DeliveryStatus.Completed;
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }
    }
}