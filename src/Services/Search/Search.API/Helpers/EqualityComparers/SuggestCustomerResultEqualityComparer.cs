using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Search.API.Commands.V1;

namespace Search.API.Helpers.EqualityComparers
{
    public class SuggestCustomerResultEqualityComparer : IEqualityComparer<SuggestCustomerResult>
    {
        public bool Equals(SuggestCustomerResult? x, SuggestCustomerResult? y)
        {
            if (x is null && y is null)
                return true;
            
            return x?.Name == y?.Name && x?.Surname == y?.Surname && x?.Email == y?.Email;
        }

        public int GetHashCode([NotNull] SuggestCustomerResult obj)
        {
            return obj.Name.GetHashCode() ^ obj.Surname.GetHashCode() ^ obj.Email.GetHashCode();
        }
    }
}