using System;
using User.Domain.Common;

namespace User.Domain
{
    public class Customer: Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public Email Email { get; private set; }
        public IdCard PersonIdCard { get; private set; }
        public IdentityType IdentityType { get; private set; }
        public string Nationality { get; private set; }
        public long Phone { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        
        public Address Address { get; private set; }
        public AddressCorrespondence CorrespondenceAddress { get; private set; }

        public Customer(string name, string surname, Email email,
            IdCard personIdCard, IdentityType identityType, string nationality, long phone,
            DateTime dateOfBirth, Address address, AddressCorrespondence correspondenceAddress)
        {
            Name = name;
            Surname = surname;
            Email = email;
            PersonIdCard = personIdCard;
            IdentityType = identityType;
            Nationality = nationality;
            DateOfBirth = dateOfBirth;
            Address = address;
            CorrespondenceAddress = correspondenceAddress;
            Phone = phone;
        }

        protected Customer()
        {
            
        }
    }
}