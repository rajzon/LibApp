using EventBus.Messages.Events;

namespace EventBus.Messages.Commands
{
    public class AddImageToBook : IntegrationEvent
    {
        public int BookId { get; init; }
        public ImageDto Image { get; init; }
    }
    
    public record ImageDto
    {
        public string Url { get; init; }
        public bool IsMain { get; init; }
    }
}