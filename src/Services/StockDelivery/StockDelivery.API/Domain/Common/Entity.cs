namespace StockDelivery.API.Domain.Common
{
    public abstract class Entity
    {
        public virtual int Id { get; protected set; }
    }
}