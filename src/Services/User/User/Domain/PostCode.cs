using User.Domain.Common;

namespace User.Domain
{
    public class PostCode : ValueObject
    {
        public string Code { get; private set; }

        public PostCode(string code)
        {
            Code = code;
        }
    }
}