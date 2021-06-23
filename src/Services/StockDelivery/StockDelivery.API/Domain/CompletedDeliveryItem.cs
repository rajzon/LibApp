using System;
using System.Collections.Generic;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Domain
{
    public class CompletedDeliveryItem : Entity, IDeliveryItem
    {
        //TODO Book should came from Book service and from instance of Book Domain
        public int BookId { get; private set;  }
        public BookEan13 BookEan { get; private set; }
        public short ItemsCount { get; private set; }


        public IReadOnlyCollection<BookStock> ItemsIds { get; private set; }
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        // Creation Date from ActiveDeliveryItem
        public CompletedDeliveryItem()
        {
            ModificationDate = DateTime.UtcNow;
        }
        
    }
}