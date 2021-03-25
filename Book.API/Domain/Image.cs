using System.Collections.Generic;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class Image : ValueObject
    {
        public string Url { get; private set; }
        public bool IsMain { get; private set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Url;
            yield return IsMain;
        }
    }
}