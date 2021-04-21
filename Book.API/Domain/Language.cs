using System;
using System.Linq;
using Book.API.Domain.Common;
using Book.API.Extensions;

namespace Book.API.Domain
{
    public class Language : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        public Language(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Language name cannot be empty or whitespace");
            
            if (! name.All(char.IsLetter))
                throw new ArgumentException("Language name must contain only letters");

            Name = name.TrimWithMultipleBetweens();
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }
        
        protected Language() { }
    }
}