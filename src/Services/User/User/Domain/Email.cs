using User.Domain.Common;

namespace User.Domain
{
    public class Email : Entity
    {
        public string EmailAddress { get; private set; }

        public Email(string email)
        {
            //TODO check if passed email is valid
            EmailAddress = email;
        }
        
        protected Email()
        {
            
        }
    }
}