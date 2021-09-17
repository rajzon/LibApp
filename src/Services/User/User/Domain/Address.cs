using User.Domain.Common;

namespace User.Domain
{
    public class Address : Entity
    {
        public string Adres { get; private set; }
        public string City { get; private set; }
        public PostCode PostCode { get; private set; }
        public string Post { get; private set; }
        public string Country { get; private set; }

        public Address(string address, string city, PostCode postCode, string post, string country)
        {
            Adres = address;
            City = city;
            PostCode = postCode;
            Post = post;
            Country = country;
        }

        protected Address()
        {
            
        }
    }
}