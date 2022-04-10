using System;
using System.Collections.Generic;
using System.Linq;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class BookIsbn10 : Entity
    {
        public string Code { get; private set; }
        
        protected BookIsbn10() { }
        public BookIsbn10(string code)
        {
            if (code?.Length != 10 || ! code.All(char.IsDigit))
                throw new ArgumentException("ISBN10 must contain exactly 10 digits");


            Code = code;
        }
    }
}