using System;
using System.Collections.Generic;
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
        
        protected ActiveDeliveryItem()
        {
        }
        
    }
}