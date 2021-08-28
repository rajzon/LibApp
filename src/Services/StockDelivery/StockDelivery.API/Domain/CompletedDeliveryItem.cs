using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Domain
{
    public class CompletedDeliveryItem : Entity, IDeliveryItem
    {
        public int BookId { get; private set;  }
        public BookEan13 BookEan { get; private set; }
        public short ItemsCount { get; private set; }
        
        private List<int> _stocks;
        
        public IEnumerable<int> Stocks => _stocks;
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        public CompletedDeliveryItem(int bookId, BookEan13 ean13, short itemsCount, IEnumerable<BookStock> stocks)
        {
            if (ean13 is null || !stocks.Any())
                throw new ArgumentException("Ean or stocks are missing");
            if (stocks.Any(s => s.BookEan13.Code != ean13.Code))
                throw new ArgumentException($"Passed Stocks contains Eans that do not match passed ean:{ean13.Code}");
            
            
            BookId = bookId;
            BookEan = ean13;
            ItemsCount = itemsCount;
            _stocks = stocks.Select(s => s.Id).ToList();
            
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }

        protected CompletedDeliveryItem()
        {
            
        }
        
    }
}