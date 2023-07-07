using System.Text;
using AddressBookApi.Entities;
using AddressBookApi.Models.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Entities
{
    public class SeedData
    {
        public static void seed(IServiceProvider serviceProvider)
        {
            AddressBookDataContext context = serviceProvider.GetRequiredService<AddressBookDataContext>();
            context.Database.EnsureCreated();
            if (!context.User.Any())
            {
                var users = new List<User>()
                {
                    new User {
                        Id = Guid.Parse("e37beb34-f626-4c9e-8988-a8bb6ca1483d"),
                        FirstName = "sneka",
                        LastName = "r",
                        EmailAddress = "sneka@propelinc.com",
                        PhoneNumber = "1234567890",
                        Role = "admin",
                        Active = true,
                        CreatedBy = Guid.NewGuid(),
                        ModifiedBy = Guid.Empty,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow,
                        UserSecret = new UserSecret() {
                            Id = Guid.Parse("8015bd0d-ccac-425d-8309-29ece7be7a04"),
                            UserName = "sneka@123",
                            Password = "e10436c0da5e451ca512f635ea25c96bf6cd12cb96b6d1b3019e004c7a644d2b",
                            Active = true
                        }
                    }
                };
                context.User.AddRange(users);
                context.SaveChanges();
            }

            if (!context.AddressBook.Any())
            {
                var addressBooks = new List<AddressBook>()
                    {
                    new AddressBook()
                    {
                        Id = Guid.Parse("c893a99f-c93d-4812-88e9-48922b039563"),
                        Name = "test Contact Details",
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow,
                        CreatedBy = Guid.NewGuid(),
                        ModifiedBy = Guid.Empty,
                        Active = true,
                        Emails = new List<Email>() { new Email
                        {
                            Id = Guid.Parse("129472d0-2247-4a8e-89b8-3e6c979285ee"),
                            EmailAddress = "test@test.com",
                            Type = "Office",
                            Active = true,
                        },},
                        Phones = new List<Phone>() { new Phone
                        {
                            Id = Guid.Parse("139472d0-2247-4a8e-89b8-3e6c979285ee"),
                            PhoneNumber = "1234567890",
                            Type = "Office",
                            Active = true,
                        },},
                        Address = new Address()
                        {
                            Id = Guid.Parse("149472d0-2247-4a8e-89b8-3e6c979285ee"),
                            Line1 = "140",
                            Line2 = "xxx",
                            City = "yyy",
                            Zipcode = "12345",
                            State = "TN",
                            Type = "Office",
                            Country = "India",
                            Active = true,
                        },
                        Asset = new Asset()
                        {
                            Id = Guid.Parse("159472d0-2247-4a8e-89b8-3e6c979285ee"),
                            File = Encoding.UTF8.GetBytes("bdkjcbsdbcjkdbjkcbakjbcjk"),
                            Active = true,
                        }
                    }
            };
                context.AddressBook.AddRange(addressBooks);
                context.SaveChanges();
            }
        }
    }
}