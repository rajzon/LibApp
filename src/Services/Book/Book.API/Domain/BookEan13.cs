using System;
using System.Collections.Generic;
using System.Linq;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class BookEan13 : Entity
    {

        private readonly string _code;
        public string Code => _code;
        
        
        public BookEan13()
        {
            _code = GenerateEan13Code();
        }
        
        //TODO: consider different way for generating EAN13
        private string GenerateEan13Code()
        {
            var rnd = new Random();
            const string digits = "1234567890";
            const int length = 13;
            return new string(Enumerable.Repeat(digits, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}