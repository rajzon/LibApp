using System;
using System.Collections.Generic;

namespace Search.API.Commands
{
    public class SearchBookCommand
    {
        public string SearchTerm { get; init; }
        public string Categories { get; init; }
        public string Authors { get; init; }
        public string Languages { get; init; }
        public string Publishers { get; init; }
        public bool? Visibility { get; init; }
        public string SortBy { get; set; }
        public int FromPage { get; set; }
        public int PageSize { get; set; }
        public DateTime ModificationDateFrom { get; init; }
        public DateTime ModificationDateTo { get; init; }
        
    }
}