using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Domain
{
    public class CompletedDelivery : Entity, IDelivery, IAggregateRoot
    {
        public string Name { get; private set; }
        
        public int ActiveDeliveryId { get; private set; }
        public DeliveryStatus DeliveryStatus { get; }
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        private readonly List<CompletedDeliveryItem> _items;
        public IReadOnlyCollection<IDeliveryItem> Items => _items.AsReadOnly();
        
        
        public CompletedDelivery(ActiveDelivery activeDelivery, IEnumerable<BookStock> stocks)
        {
            if (!activeDelivery.IsAllDeliveryItemsScanned)
                throw new ArgumentException(
                    "All delivery items must be scanned to be able to move Active Delivery to Completed");

            Name = activeDelivery.Name;
            DeliveryStatus = DeliveryStatus.Completed;
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;

            ActiveDeliveryId = activeDelivery.Id;
            _items = activeDelivery.Items
                .Select(i => new CompletedDeliveryItem(i.BookId, i.BookEan,
                    i.ItemsCount, GetStocksByEan(i.BookEan, stocks))).ToList();
            
        }

        private List<BookStock> GetStocksByEan(BookEan13 ean13, IEnumerable<BookStock> stocks)
        {
            return stocks.Where(s => s.BookEan13?.Code == ean13?.Code).ToList();
        }

        protected CompletedDelivery()
        {
            
        }
    }
}