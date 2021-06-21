using System;
using System.Collections.ObjectModel;

namespace StockDelivery.API.Domain
{
    public interface IDelivery
    {
        public string Name { get; }
        public DeliveryStatus DeliveryStatus { get; }
        
        public ReadOnlyCollection<IDeliveryItem> Items { get; }
        
        public DateTime ModificationDate { get; }
        public DateTime CreationDate { get; }
    }
}