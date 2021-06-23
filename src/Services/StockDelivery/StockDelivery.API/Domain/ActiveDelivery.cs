using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public class ActiveDelivery : Entity, IDelivery, IAggregateRoot
    {
        public string Name { get; private set; }
        public bool IsAllDeliveryItemsScanned { get; private set; }

        public DeliveryStatus DeliveryStatus { get; }
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        
        private readonly List<ActiveDeliveryItem> _items;
        public IReadOnlyCollection<IDeliveryItem> Items => _items;
        
        public ActiveDelivery(string name)
        {
            Name = name;
            _items ??= new List<ActiveDeliveryItem>();

            DeliveryStatus = DeliveryStatus.Active;
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;

        }
        
        protected ActiveDelivery()
        {
        }

        public void AddDeliveryItem(int bookId, string bookEan, short itemsCount)
        {
            _items.Add(new ActiveDeliveryItem(bookId, bookEan, itemsCount));
        }

    }
}