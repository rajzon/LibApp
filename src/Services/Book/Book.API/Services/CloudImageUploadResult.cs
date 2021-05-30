using System.Collections.Generic;
using Book.API.Domain.Errors;

namespace Book.API.Services
{
    public class CloudImageUploadResult
    {
        public bool Succeeded { get; private set; }
        public string Url { get; private set; }
        public string PublicId { get; private set; }
        public Error Error { get; private set; }
        
        
        public CloudImageUploadResult(bool succeeded, string url, string publicId)
        {
            Succeeded = succeeded;
            Url = url;
            PublicId = publicId;
        }
        
        public CloudImageUploadResult(bool succeeded, Error errors)
        {
            Error = errors;
        }

    }
}