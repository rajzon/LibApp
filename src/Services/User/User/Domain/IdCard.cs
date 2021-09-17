using User.Domain.Common;

namespace User.Domain
{
    public class IdCard : Entity
    {
        public string Value { get; private set; }
        public IdCard(string value, IdentityType identityType)
        {
            //TODO consider different check based on identity type
            Value = value;
        }
        
        protected IdCard()
        {
            
        }
    }
}