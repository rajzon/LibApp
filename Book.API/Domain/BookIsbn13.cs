using System;
using System.Collections.Generic;
using System.Linq;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class BookIsbn13 : ValueObject
    {
        public string Code { get; private set; }
        
        public BookIsbn13(string code)
        {
            if (code?.Length != 10)
                throw new ArgumentException("ISBN13 must contain exactly 13 digits");
            if (! (code ?? string.Empty).All(char.IsDigit))
                throw new ArgumentException("ISBN13 must contain only digits");
            
            Code = code;
        }
        
        protected BookIsbn13() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}