using System;
using System.Collections.Generic;
using Book.API.Domain.Common;
using Book.API.Extensions;

namespace Book.API.Domain
{
    public class Category : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        
        //DDD violation: Navigation property to outside aggregate in order to create many-to-many relationship
        public IReadOnlyCollection<Book> Books { get; private set; }
        
        public Category(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty or whitespace");

            Name = name.TrimWithMultipleBetweens();
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }
        
        protected Category() { }
    }
}