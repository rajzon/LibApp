using System;
using System.Collections.Generic;
using System.Linq;
using Book.API.Domain.Common;
using Book.API.Extensions;

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
        
        public string FullName { get; private set; }
        
        
        public AuthorName(string firstName, string lastName)
            :this($"{firstName} {lastName}")
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Firstname cannot be empty or whitespace");
            if (firstName.Any(char.IsDigit))
                throw new ArgumentException("Firstname cannot contain any digits");
            
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Lastname cannot be empty or whitespace");
            if (lastName.Any(char.IsDigit))
                throw new ArgumentException("Lastname cannot contain any digits");
            
            
            FirstName = firstName.TrimWithMultipleBetweens();
            LastName = lastName.TrimWithMultipleBetweens();
        }

        public AuthorName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Fullname cannot be empty or whitespace");
            if (fullName.Any(char.IsDigit))
                throw new ArgumentException("Fullname cannot contain any digits");
            
            
            FullName = fullName.TrimWithMultipleBetweens();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return FullName;
        }
    }
}