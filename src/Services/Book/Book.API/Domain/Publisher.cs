using System;
using Book.API.Domain.Common;
using Book.API.Extensions;

namespace Book.API.Domain
{
    public class Publisher : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        
        public Publisher(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Publisher name cannot be empty or whitespace");
            
            Name = name.TrimWithMultipleBetweens();
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }
    }
}