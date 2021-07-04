using System;
using System.Linq;
using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain.ValueObjects
{
    public class BookEan13 : ValueObject
    {
        private readonly string _code;
        public string Code => _code;
        
        public BookEan13(string code)
        {
            if (code.Length != 13)
                throw new ArgumentException("EAN must be exactly 13 characters");
            if (!code.All(char.IsDigit))
                throw new AggregateException("EAN must contains only digits");

            _code = code;
        }
        
    }
}