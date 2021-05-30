using System.Collections.Generic;

namespace Search.API.Application.Services.Common
{
    public enum FieldType
    {
        Keyword,
        NonKeyword
    }

    public enum DefaultSorting
    {
        Ascending,
        Descending
    }
    
    public static class BookRepositorySettings
    {
        public static Dictionary<string, FieldType> BOOK_MANAGEMENT_SORT_ALLOWED_VALUES =
            new() { {"title", FieldType.Keyword}, {"modificationDate", FieldType.NonKeyword} };

        public static DefaultSorting DEFAULT_SORTING = DefaultSorting.Descending;
        public static int DEFAULT_PAGINATION_PAGE_SIZE = 10;
    }
}