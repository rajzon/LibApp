using System;
using System.Collections.ObjectModel;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public class ActiveDelivery : IDelivery, IAggregateRoot
    {
        public string Name { get; private set; }
        public DeliveryStatus DeliveryStatus { get; }
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        public ReadOnlyCollection<IDeliveryItem> Items { get; private set; }
        
        public ActiveDelivery()
        {
            DeliveryStatus = DeliveryStatus.Active;
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;

        }

    }
}