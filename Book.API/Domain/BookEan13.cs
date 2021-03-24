using System;
using System.Linq;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class BookEan13 : ValueObject
    {
        public string Code { get; private set; }

        protected BookEan13() { }
            
        
        public BookEan13(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("EAN13 must not be empty or whitespace");
            if (code.Length < 10)
                throw new ArgumentException("EAN13 must not exceed 13 digits");
            if (! code.All(char.IsDigit))
                throw new ArgumentException("EAN13 must contain only digits");
            Code = code;
        }

    }
}