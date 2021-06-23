using System.Collections.Generic;

namespace EventBus.Messages.Commands
{
    public class CheckBooksExsitance
    {
        public Dictionary<int,string> BooksIdsWithEans { get; init; }
    }


}