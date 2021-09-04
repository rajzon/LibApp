using Nest;
using Search.API.Domain;

namespace Search.API.Commands.V1
{
    public class SuggestCustomerResult
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public SuggestCustomerResult(ISuggestOption<Customer> opts)
        {
            if (opts is null) return;
            Name = opts.Source.Name;
            Surname = opts.Source.Surname;
            Email = opts.Source.Email?.EmailAddress;
        }
    }
}