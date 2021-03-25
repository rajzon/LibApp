using System;
using System.Collections.Generic;
using System.Linq;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class Author : Entity, IAggregateRoot
    {
        public AuthorName Name { get; private set; }
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        
        public Author(AuthorName name)
        {
            Name = name ?? throw new ArgumentException("AuthorName cannot be null");
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }
        
        protected Author() { }

    }

    public class AuthorName : ValueObject
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }
        
        public AuthorName(string firstName, string lastName)
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
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}