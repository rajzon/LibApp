using System;
using System.Collections.Generic;
using System.Linq;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class BookIsbn10 : ValueObject
    {
        public string Code { get; private set; }
        
        protected BookIsbn10() { }
        public BookIsbn10(string code = null)
        {
            if (code?.Length < 10)
                throw new ArgumentException("ISBN10 must not exceed 10 digits");
            if (! (code ?? string.Empty).All(char.IsDigit))
                throw new ArgumentException("ISBN10 must contain only digits");


            Code = code;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}