using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public class ActiveDelivery : Entity, IDelivery, IAggregateRoot
    {
        public string Name { get; private set; }
        public bool IsAllDeliveryItemsScanned { get; private set; }
        public bool IsAnyDeliveryItemsScanned { get; private set; }

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
            if (_items is null)
                return;

            if (_items.Any(i => i.BookId.Equals(bookId)))
                return;

            _items.Add(new ActiveDeliveryItem(bookId, bookEan, itemsCount));
            ModificationDate = _items.LastOrDefault() is not null
                ? _items.LastOrDefault().ModificationDate
                : ModificationDate;
        }

        public void EditDeliveryItemsCount(int itemId, short itemsCount)
        {
           var item = _items.FirstOrDefault(i => i.Id.Equals(itemId));
           
           item?.EditItemsCount(itemsCount);
        }

        /// <summary>
        /// It will delete Deliver that exist in Db and DO NOT exist in passed argument collection
        /// </summary>
        /// <param name="itemIdsThatPotentiallyMissIds"></param>
        public void DeleteMissingDeliveryItems(IEnumerable<int> itemIdsThatPotentiallyMissIds)
        {
            var itemsToRemove = GetItemsThatAreMissingInArg(itemIdsThatPotentiallyMissIds).ToList();
            foreach (var item in itemsToRemove)
            {
                _items.Remove(item);
            }
        }

        private IEnumerable<ActiveDeliveryItem> GetItemsThatAreMissingInArg(
            IEnumerable<int> itemIdsThatPotentiallyMissIds)
        {
            return _items.Where(i => !itemIdsThatPotentiallyMissIds.Contains(i.Id));
        }

    }
}