using System;
using System.Collections.Generic;
using System.Linq;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class BookIsbn13 : Entity
    {
        public string Code { get; private set; }
        
        public BookIsbn13(string code)
        {
            if (code?.Length != 13 || ! code.All(char.IsDigit))
                throw new ArgumentException("ISBN13 must contain exactly 13 digits");

            Code = code;
        }
        
        protected BookIsbn13() { }
    }
}