using System;
using System.Collections.Generic;
using System.Linq;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public class CancelledDelivery : Entity, IDelivery, IAggregateRoot
    {
        public string Name { get; private set; }
        
        public int ActiveDeliveryId { get; private set; }
        
        public DeliveryStatus DeliveryStatus { get; }
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        private readonly List<CancelledDeliveryItem> _items;
        public IReadOnlyCollection<IDeliveryItem> Items => _items;
        public string CancellationReason { get; private set; }
        
        
        public CancelledDelivery(ActiveDelivery activeDelivery, string cancellationReason = default)
        {
            if (activeDelivery.IsAnyDeliveryItemsScanned)
                throw new ArgumentException(
                    "You cannot move active delivery to cancelled if there is any scanned items");

            DeliveryStatus = DeliveryStatus.Cancelled;
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;

            ActiveDeliveryId = activeDelivery.Id;
            _items = activeDelivery.Items
                .Select(i => new CancelledDeliveryItem(i.BookId, i.BookEan, i.ItemsCount)).ToList();
            CancellationReason = cancellationReason;
            
        }

        protected CancelledDelivery()
        {
            
        }
    }
}