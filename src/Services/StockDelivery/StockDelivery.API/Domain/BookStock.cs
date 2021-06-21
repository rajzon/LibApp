using System;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Domain
{
    public class BookStock : Entity, IAggregateRoot
    {
        public StockId StockId { get; private set; }
        //TODO BookEan should came from Book service and from instance of Book Domain
        public BookEan BookEan { get; private set; }
        //TODO Book should came from Book service and from instance of Book Domain
        public int BookId { get; private set; }
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}