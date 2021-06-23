using System.Collections.Generic;
using System.Linq;
using Nest;
using Search.API.Domain;

namespace Search.API.Commands.V1
{
    public class SuggestBookManagementResult
    {
        public string Title { get; init; }
        public IEnumerable<string> Authors { get; init; }
        public IEnumerable<string> Categories { get; init; }

        public SuggestBookManagementResult(ISuggestOption<Book> opts)
        {
            if (opts is null) return;
            Title = opts.Source.Title;
            Authors = opts.Source.Authors.Select(a => a.Name.FullName);
            Categories = opts.Source.Categories.Select(c => c.Name);


        }
    }
}