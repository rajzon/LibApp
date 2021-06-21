using System;
using System.Collections.ObjectModel;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public class CancelledDelivery : IDelivery, IAggregateRoot
    {
        public string Name { get; private set; }
        public DeliveryStatus DeliveryStatus { get; }
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        public ReadOnlyCollection<IDeliveryItem> Items { get; private set; }
        public string CancellationReason { get; private set; }
        
        

        public CancelledDelivery(string cancellationReason)
        {
            DeliveryStatus = DeliveryStatus.Cancelled;
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;

            CancellationReason = cancellationReason;
        }
    }
}