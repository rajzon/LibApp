using System;

namespace EventBus.Messages.Events
{
    public class IntegrationEvent
    {
        public Guid EventId { get; private set; }
        public DateTime EventCreationDate { get; private set; }

        public IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            EventCreationDate = DateTime.UtcNow;
        }

        public IntegrationEvent(Guid eventId, DateTime creationDate)
        {
            EventId = eventId;
            EventCreationDate = creationDate;
        }
        
    }
}