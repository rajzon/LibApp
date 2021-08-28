using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Domain.ValueObjects;

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

        public void ScanItem(string ean)
        {
            if (string.IsNullOrEmpty(ean))
                throw new ArgumentException("Ean cannot be empty");
            if (!_items.Any())
                throw new ArgumentException("There is none items in Delivery");
            
            var itemToScan = _items.FirstOrDefault(i => i.BookEan.Code.Equals(ean));
            if (itemToScan is null)
                throw new ArgumentException($"There is no items in Delivery: {Id} with EAN:{ean}");
            
            itemToScan.Scan();
            ChangeScannedItemStatus();
        }

        public void UnscanItem(string ean)
        {
            if (string.IsNullOrEmpty(ean))
                throw new ArgumentException("Ean cannot be empty");
            if (!_items.Any())
                throw new ArgumentException("There is none items in Delivery");
            
            var itemToUnscan = _items.FirstOrDefault(i => i.BookEan.Code.Equals(ean));
            if (itemToUnscan is null)
                throw new ArgumentException($"There is no items in Delivery: {Id} with EAN:{ean}");
            
            itemToUnscan.Unscan();
            ChangeScannedItemStatus();
        }

        public bool IsScanOperationAllowed(bool scanMode, string ean, out List<string> errors)
        {
            errors = new List<string>();
            var itemToCheck = _items.FirstOrDefault(i => i.BookEan.Code.Equals(ean));
            if (itemToCheck is null)
                throw new ArgumentException($"Not found any item with ean {ean}");
            
            if (scanMode && IsAllDeliveryItemsScanned)
                errors.Add($"Delivery Item: {Id} have all items scanned");
            
            if (!scanMode && !IsAnyDeliveryItemsScanned)
                errors.Add($"Delivery Item: {Id} do not have any items scanned, so you cannot Unscan");

            if(!itemToCheck.IsScanOperationAllowed(scanMode, out List<string> errors2))
                errors = errors.Union(errors2).ToList();
            
            if (errors.Any())
                return false;

            return true;
        }
        
        public bool IsRedeemOperationAllowed()
        {
            return IsAllDeliveryItemsScanned;
        }

        private void ChangeScannedItemStatus()
        {
            if (!_items.Any())
                throw new ArgumentException("There is none items in Delivery");

            IsAnyDeliveryItemsScanned = _items.Any(i => i.IsScanned);

            IsAllDeliveryItemsScanned = _items.All(i => i.IsAllScanned);
        }

        private IEnumerable<ActiveDeliveryItem> GetItemsThatAreMissingInArg(
            IEnumerable<int> itemIdsThatPotentiallyMissIds)
        {
            return _items.Where(i => !itemIdsThatPotentiallyMissIds.Contains(i.Id));
        }

    }

    public class RedeemedDeliveryDomainEvent : INotification
    {
        public Dictionary<BookEan13, short> BookEansWithCount { get; init; }

        public RedeemedDeliveryDomainEvent(Dictionary<BookEan13, short> bookEansWithCount)
        {
            BookEansWithCount = bookEansWithCount;
        }
    }
}