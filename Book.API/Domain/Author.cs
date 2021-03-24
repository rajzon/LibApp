using System;
using System.Linq;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class Author : Entity, IAggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        public Author(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Firstname cannot be empty or whitespace");
            if (! firstName.All(char.IsLetter))
                throw new ArgumentException("Firstname must contain only letters");
            
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Lastname cannot be empty or whitespace");
            if (! lastName.All(char.IsLetter))
                throw new ArgumentException("Lastname must contain only letters");
                
            
            
            FirstName = firstName;
            LastName = lastName;
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }

    }
}