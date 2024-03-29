﻿using System;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Domain
{
    public class CancelledDeliveryItem : Entity, IDeliveryItem
    {
        //TODO Book should came from Book service and from instance of Book Domain
        public int BookId { get; private set;  }
        public BookEan13 BookEan { get; private set; }
        public short ItemsCount { get; private set; }

        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        // Creation Date from ActiveDeliveryItem
        public CancelledDeliveryItem(int bookId, BookEan13 ean13, short itemsCount)
        {
            BookId = bookId;
            BookEan = new BookEan13(ean13.Code);
            ItemsCount = itemsCount;
            
            ModificationDate = DateTime.UtcNow;  
            CreationDate = DateTime.UtcNow;  
        }
        
        protected CancelledDeliveryItem()
        {
        }
    }
}