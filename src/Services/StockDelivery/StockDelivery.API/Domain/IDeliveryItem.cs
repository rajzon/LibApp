using System;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Domain
{
    public interface  IDeliveryItem
    {
        //TODO Book should came from Book service and from instance of Book Domain
        public int BookId { get; }
        public BookEan13 BookEan { get; }
        public short ItemsCount { get; }


        public DateTime ModificationDate { get; }
        public DateTime CreationDate { get; }
    }
}