namespace GameShop.Models
{
    internal class Address
    {
        public int Id { get; internal set; }
        public string Street { get; internal set; }
        public string Postcode { get; internal set; }
        public string City { get; internal set; }

        public int customerId { get; internal set; }

        public Customer Customer { get; internal set; }

        public Address(string street, string postcode, string city)
        {
            this.Street = street;
            this.Postcode = postcode;
            this.City = city;
        }

        public Address(int id, string street, string postcode, string city, int customerId)
        {
            Id = id;
            Street = street;
            Postcode = postcode;
            City = city;
            this.customerId = customerId;
        }


    }
}