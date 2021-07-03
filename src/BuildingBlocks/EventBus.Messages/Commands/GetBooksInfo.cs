using System.Collections.Generic;

namespace EventBus.Messages.Commands
{
    public class GetBooksInfo
    {
        public List<int> BooksIds { get; init; }
    }
}