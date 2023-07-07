using System;
using AddressBookApi.Entities;
using AddressBookApi.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace AddressBookApi.Test;

public static class ContextFactory
{
    public static DbContextOptions<AddressBookDataContext> DbContextOptionsInMemory()
    {
        var options = new DbContextOptionsBuilder<AddressBookDataContext>()
            .UseInMemoryDatabase("InMemoryDbForTesting")
            .Options;

        return options;
    }

    public static async void CreateDataBaseInMemory(DbContextOptions<AddressBookDataContext> options)
    {
        await using (var context = new AddressBookDataContext(options))
        {
            CreateData(context);
        }
    }

    public static async Task CleanDataBase(DbContextOptions<AddressBookDataContext> options)
    {
        await using (var context = new AddressBookDataContext(options))
        {
            foreach (var user in context.User)
                context.User.Remove(user);
            await context.SaveChangesAsync();
        }

        await using (var context = new AddressBookDataContext(options))
        {
            foreach (var addressBook in context.AddressBook)
                context.AddressBook.Remove(addressBook);
            await context.SaveChangesAsync();
        }
    }

    private static void CreateData(AddressBookDataContext DbContext)
    {
        DbContext.User.Add(new User { Id = Guid.Parse("e37beb34-f626-4c9e-8988-a8bb6ca1483d"), FirstName = "Test", LastName = "Micheal", EmailAddress = "Test@test.com", PhoneNumber = "1234567890", Role = "admin", Active = true, CreatedBy = Guid.NewGuid(), ModifiedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow, UserSecret = new UserSecret() { Id = Guid.Parse("8015bd0d-ccac-425d-8309-29ece7be7a04"), UserName = "Test123", Password = "e10436c0da5e451ca512f635ea25c96bf6cd12cb96b6d1b3019e004c7a644d2b", Active = true } }
            );
        DbContext.SaveChangesAsync();
        DbContext.User.Add(new User { Id = Guid.Parse("4c066687-6c57-4ef1-94ea-32f3c3786797"), FirstName = "Test", LastName = "Micheal", EmailAddress = "Test@testt.com", PhoneNumber = "1234567890", Role = "admin", Active = true, CreatedBy = Guid.NewGuid(), ModifiedBy = Guid.Empty, CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow, UserSecret = new UserSecret() { Id = Guid.Parse("b45c7643-1876-49d4-b0eb-773515aa9227"), UserName = "Test1223", Password = "e10436c0da5e451ca512f635ea25c96bf6cd12cb96b6d1b3019e004c7a644d2b", Active = true } }
            );
        DbContext.SaveChangesAsync();
        DbContext.AddressBook.Add(new AddressBook
        {
            Id = Guid.Parse("c893a99f-c93d-4812-88e9-48922b039563"),
            Name = "Test Contact Details",
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            ModifiedBy = Guid.Empty,
            Active = true,
            Emails = new List<Email>() { new Email
            {
                Id = Guid.Parse("129472d0-2247-4a8e-89b8-3e6c979285ee"),
                EmailAddress = "Test@test.com",
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
        });
        DbContext.SaveChangesAsync();
        DbContext.AddressBook.Add(new AddressBook
        {
            Id = Guid.Parse("119172d0-2247-4a8e-89b8-3e6c979285ee"),
            Name = "Test Personal Contact Details",
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            ModifiedBy = Guid.Empty,
            Active = true,
            Emails = new List<Email>() { new Email
            {
                Id = Guid.Parse("129272d0-2247-4a8e-89b8-3e6c979285ee"),
                EmailAddress = "Test@test.com",
                Type = "Office",
                Active = true,
            },},
            Phones = new List<Phone>() { new Phone
            {
                Id = Guid.Parse("139372d0-2247-4a8e-89b8-3e6c979285ee"),
                PhoneNumber = "1234567890",
                Type = "Office",
                Active = true,
            },},
            Address = new Address()
            {
                Id = Guid.Parse("149572d0-2247-4a8e-89b8-3e6c979285ee"),
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
                Id = Guid.Parse("159672d0-2247-4a8e-89b8-3e6c979285ee"),
                File = Encoding.UTF8.GetBytes("bdkjcbsdbcjkdbjkcbakjbcjk"),
                Active = true,
            }
        });

        DbContext.SaveChangesAsync();
    }
}
