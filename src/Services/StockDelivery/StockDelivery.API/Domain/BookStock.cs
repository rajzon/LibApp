using System;
using System.Linq;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Domain
{
    public class BookStock : Entity, IAggregateRoot
    {
        public BookEan13ForStock BookEan13 { get; private set; }
        public int BookId { get; private set; }
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }

        public BookStock(ActiveDelivery activeDelivery, IDeliveryItem item)
        {

            if (item is not ActiveDeliveryItem)
                throw new ArgumentException($"Passed Delivery Item is not of type {nameof(ActiveDeliveryItem)}");
            if (item is null)
                throw new ArgumentException("DeliveryItem cannot be null");
            if (activeDelivery is null)
                throw new ArgumentException("ActiveDelivery cannot be null");
            if (!activeDelivery.Items.Any(i => i.BookEan.Code.Equals(item.BookEan.Code)))
                throw new ArgumentException($"ActiveDelivery: {activeDelivery.Id} do not contain ean {item.BookEan.Code}");

            BookEan13 = new BookEan13ForStock(item.BookEan.Code);
            BookId = item.BookId;
            
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }

        protected BookStock()
        {
            
        }
    }
}