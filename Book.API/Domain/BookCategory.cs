using System;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class BookCategory : Entity, IAggregateRoot
    {
        public int BookId { get; private set; }
        public int CategoryId { get; private set; }

        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}