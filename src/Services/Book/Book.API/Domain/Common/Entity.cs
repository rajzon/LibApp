using System.Collections.Generic;
using MediatR;

namespace Book.API.Domain.Common
{
    public abstract class Entity
    {
        public virtual int Id { get; protected set; }
    }
}