using System.Text.RegularExpressions;

namespace Book.API.Extensions
{
    public static class StringExtensions
    {
        public static string TrimWithMultipleBetweens(this string self)
        {
            var trimmedValue = self.Trim();
            
            return Regex.Replace(trimmedValue, @"\s+", " ");
        }
    }
}