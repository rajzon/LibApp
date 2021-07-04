using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StockDelivery.API.Domain
{
    public interface IDelivery
    {
        public int Id { get; }
        public string Name { get; }

        public DeliveryStatus DeliveryStatus { get; }
        
        public IReadOnlyCollection<IDeliveryItem> Items { get; }
        
        public DateTime ModificationDate { get; }
        public DateTime CreationDate { get; }
    }
}