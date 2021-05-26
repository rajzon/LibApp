using System.Collections.Generic;
using System.Linq;
using Nest;
using Search.API.Application.Services.Common;
using FieldType = Search.API.Application.Services.Common.FieldType;

namespace Search.API.Extensions
{
    public static class SortDescriptorExtensions
    {
        public static SortDescriptor<T> Custom<T>(this SortDescriptor<T> ss, string sortBy,
            Dictionary<string, FieldType> sortAllowedValues) where T : class
        {
            var sortValue = sortBy?.Split(":");
            string selectedSortingField =  sortValue?.ElementAtOrDefault(0) ?? string.Empty;
            string sortingType = sortValue?.ElementAtOrDefault(1) ?? string.Empty;
            
            var isSelectedFieldExist = sortAllowedValues.TryGetValue(selectedSortingField, out var fieldType);
            
            if (! isSelectedFieldExist ||
                string.IsNullOrEmpty(sortingType))
                return ss;

            if (fieldType.Equals(FieldType.NonKeyword))
            {
                ss = Sort(ss, sortingType, selectedSortingField);
            }
            else
            {
                selectedSortingField += ".keyword";
                ss = Sort(ss, sortingType, selectedSortingField);
            }
            

            return ss;
        }

        private static SortDescriptor<T> Sort<T>(SortDescriptor<T> ss, string sortingType, string selectedSortingField)
            where T : class
        {
            switch (sortingType)
            {
                case "desc":
                    ss.Descending(selectedSortingField);
                    break;
                case "asc":
                    ss.Ascending(selectedSortingField);
                    break;
                default:
                    ss = BookRepositorySettings.DEFAULT_SORTING.Equals(DefaultSorting.Descending)
                        ? ss.Descending(selectedSortingField)
                        : ss.Ascending(selectedSortingField);
                    break;
            }

            return ss;
        }
    }
}