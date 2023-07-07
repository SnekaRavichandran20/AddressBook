using AddressBookApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApi.Models.Data
{
    public class AddressBookDataContext : DbContext
    {
        public AddressBookDataContext(DbContextOptions<AddressBookDataContext> options) : base(options)
        {}
        public DbSet<AddressBook> AddressBook { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserSecret> UserSecret { get; set; }
    }
}