using System.Collections.Generic;

namespace GameShop.Models
{
    internal class Customer
    {
        public int Id { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string SocialSecurityNumber { get; internal set; }
        public Address Address { get; internal set; }

        public Customer(string firstName, string lastName, string socialSecurityNumber, Address address)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.SocialSecurityNumber = socialSecurityNumber;
            this.Address = address;
        }

        public Customer()
        {
            
        }

        public List<Order> Orders { get; internal set; } = new List<Order>();
    }
}