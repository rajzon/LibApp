using System;
using System.Collections.Generic;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class Book : Entity
    {
        public string Title { get; private set; }

        
        public Book(string title)
        {
            Title = title;
        }
        

        public void GenerateEANCode()
        {
            //validate input
        }
    }
}