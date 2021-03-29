using System.Collections.Generic;
using Book.API.Domain.Errors;

namespace Book.API.Services
{
    public class CloudImageUploadResult
    {
        public string Url { get; private set; }
        public string PublicId { get; private set; }
        public Error Error { get; private set; }
        
        
        public CloudImageUploadResult(string url, string publicId)
        {
            Url = url;
            PublicId = publicId;
        }
        
        public CloudImageUploadResult(Error errors)
        {
            Error = errors;
        }

    }
}