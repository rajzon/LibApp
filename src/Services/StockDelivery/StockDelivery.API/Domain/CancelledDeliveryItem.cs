﻿using System;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain
{
    public class CancelledDeliveryItem : Entity, IDeliveryItem
    {
        //TODO Book should came from Book service and from instance of Book Domain
        public int BookId { get; private set;  }
        public short ItemsCount { get; private set; }
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        // Creation Date from ActiveDeliveryItem
        public CancelledDeliveryItem()
        {
            ModificationDate = DateTime.UtcNow;   
        }    
    }
}