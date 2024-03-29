﻿using System;
using System.Collections.Generic;
using System.Linq;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Domain
{
    public class ActiveDeliveryItem : Entity, IDeliveryItem
    {
        //TODO Book should came from Book service and from instance of Book Domain
        public int BookId { get; private set;  }
        public BookEan13 BookEan { get; private set; }
        public short ItemsCount { get; private set; }
        
        public short ScannedCount { get; set; }
        public bool IsScanned { get; private set; }
        public bool IsAllScanned { get; private set; }


        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }


        public ActiveDeliveryItem(int bookId, string bookEan, short itemsCount = 1)
        {
            BookId = bookId;
            BookEan = new BookEan13(bookEan);
            ItemsCount = itemsCount.Equals(0)? (short) 1: itemsCount;
            
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
            
        }

        public void EditItemsCount(short itemsCount)
        {
            ItemsCount = itemsCount;
            ModificationDate = DateTime.UtcNow;
        }

        public void Scan()
        {
            if (ItemsCount < 1)
                throw new ArgumentException($"Cannot scan Item(EAN: {BookEan.Code}) that do not have any positions");
            if (IsAllScanned)
                throw new ArgumentException($"All positions of Item(EAN: {BookEan.Code}) are scanned");
            if (ScannedCount >= ItemsCount)
                throw new ArgumentException($"Cannot scan more positions for Item(EAN: {BookEan.Code}) because all positions are already scanned");
            
            ScannedCount++;
            
            if (ScannedCount > 0)
                IsScanned = true;
            
            if (ScannedCount.Equals(ItemsCount))
                IsAllScanned = true;
        }
        
        public void Unscan()
        {
            if (ItemsCount < 1)
                throw new ArgumentException($"Cannot unscan Item(EAN: {BookEan.Code}) that do not have any positions");
            if (! IsScanned)
                throw new ArgumentException($"Cannot unscan Item(EAN: {BookEan.Code}) that do not have any scanned positions");
           
            ScannedCount--;
            
            if (ScannedCount.Equals(0))
                IsScanned = false;
            if (ScannedCount < ItemsCount)
                IsAllScanned = false;
        }

        public bool IsScanOperationAllowed(bool scanMode, out List<string> errors)
        {
            errors = new List<string>();
            if (scanMode && IsAllScanned)
                errors.Add($"Item(EAN{BookEan.Code}) have all items scanned");

            if (!scanMode && !IsScanned)
                errors.Add($"Item(EAN{BookEan.Code}) is not scanned, so you cannot Unscan");

            if (errors.Any())
                return false;

            return true;
        }
        
        protected ActiveDeliveryItem()
        {
        }
        
    }
}