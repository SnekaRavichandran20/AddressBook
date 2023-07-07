using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBookApi.Entities
{
    public class AddressBook : BaseModel
    {
        public string Name { get; set; }
        public List<Email> Emails { get; set; }
        public Address Address { get; set; }
        public List<Phone> Phones { get; set; }
        public Asset Asset { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }

    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public bool Active { get; set; }
        [ForeignKey("AddressBook")]
        public Guid AddressBookId { get; set; }
        public AddressBook AddressBook { get; set; }
    }

    public class Asset
    {
        [Key]
        public Guid Id { get; set; }
        public byte[] File { get; set; }
        public bool Active { get; set; }
        [ForeignKey("AddressBook")]
        public Guid AddressBookId { get; set; }
        public AddressBook AddressBook { get; set; }
    }

    public class Email
    {
        [Key]
        public Guid Id { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }
        public bool Active { get; set; }
        public string Type { get; set; }
        [ForeignKey("AddressBook")]
        public Guid AddressBookId { get; set; }
        public AddressBook AddressBook { get; set; }
    }

    public class Phone
    {
        [Key]
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
        [ForeignKey("AddressBook")]
        public Guid AddressBookId { get; set; }
        public AddressBook AddressBook { get; set; }
    }
}


