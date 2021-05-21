using System;
using System.Collections.Generic;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class Image : Entity
    {
        public string Url { get; private set; }
        public string PublicId { get; private set; }
        public bool IsMain { get; private set; }
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }

        public Image(string url, string publicId, bool isMain = default)
        {
            if (!IsValidUrl(url))
                throw new ArgumentException("Passed string is not valid url");
            if (string.IsNullOrWhiteSpace(publicId))
                throw new ArgumentException("PublicId cannot be empty or whitespace");
            
            Url = url;
            PublicId = publicId;
            IsMain = isMain;
            
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }

         static bool IsValidUrl(string url)
         {
             return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uriResult);
         }
    }
}