using AddressBookApi.Entities;
using AddressBookApi.Models.Data;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApi.Repositories
{
    public class AddressBookRepository : IAddressBookRepository
    {
        private readonly AddressBookDataContext _dbContext;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor for injecting logger and db context
        /// </summary>
        /// <param name="logger">logger object</param>
        /// <param name="dbContext">The db context object</param>
        public AddressBookRepository(ILoggerManager logger, AddressBookDataContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// creates addressbook in the database
        /// </summary>
        /// <param name="addressbook">the addressbook model that has to be created</param>
        /// <returns></returns>
        public Guid CreateAddressBook(AddressBook addressBook)
        {
            _dbContext.AddressBook.Add(addressBook);
            _dbContext.SaveChanges();
            return addressBook.Id;
        }

        /// <summary>
        /// fetch addressbook from the database
        /// </summary>
        /// <returns></returns>
        public List<AddressBook> GetAddressBook()
        {
            List<AddressBook> addressBook = _dbContext.AddressBook
                                .Include(addressBook => addressBook.Emails)
                                .Include(addressBook => addressBook.Phones)
                                .Include(addressBook => addressBook.Address)
                                .Include(addressBook => addressBook.Asset)
                                .Where(addressBook => addressBook.Active).ToList();
            return addressBook;
        }

        /// <summary>
        /// fetch addressbook by its id from the database
        /// </summary>
        /// <param name="id">addressbook id</param>
        /// <returns></returns>
        public AddressBook GetAddressBookById(Guid id)
        {
            AddressBook addressBook = _dbContext.AddressBook
                                .Include(addressBook => addressBook.Emails)
                                .Include(addressBook => addressBook.Phones)
                                .Include(addressBook => addressBook.Address)
                                .Include(addressBook => addressBook.Asset)
                                .FirstOrDefault(addressBook => addressBook.Id == id && addressBook.Active);
            return addressBook;
        }

        /// <summary>
        /// update addressbook in the database
        /// </summary>
        /// <param name="id">addressbook id</param>
        /// <param name="addressbook">the addressbook model that has to be updated</param>
        /// <returns></returns>
        public bool UpdateAddressBook(Guid id, AddressBook addressBook)
        {
            AddressBook oldDataOfAddressBook = GetAddressBookById(id);
            if (oldDataOfAddressBook is null)
            {
                return false;
            }
            _dbContext.Entry(oldDataOfAddressBook).CurrentValues.SetValues(addressBook);
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// deletes addressbook in the database
        /// </summary>
        /// <param name="id">addressbook id</param>
        /// <returns></returns>
        public bool DeleteAddressBook(Guid id)
        {
            AddressBook addressBookToDelete = GetAddressBookById(id);
            if (addressBookToDelete != null)
            {
                addressBookToDelete.Active = false;
                addressBookToDelete.Address.Active = false;
                addressBookToDelete.Emails.Select(x => x.Active = false);
                addressBookToDelete.Phones.Select(x => x.Active = false);
                addressBookToDelete.Asset.Active = false;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// get the addressbook count
        /// </summary>
        /// <returns></returns>
        public int GetAddressBookCount()
        {
            return _dbContext.AddressBook
                            .Include(addressBook => addressBook.Emails)
                            .Include(addressBook => addressBook.Phones)
                            .Include(addressBook => addressBook.Address)
                            .Where(addressBook => addressBook.Active)
                            .Count();
        }

        /// <summary>
        /// check for conflict of addressbookname
        /// </summary>
        /// <param name="addressbookName">addressbookname that needs to check for conflict</param>
        /// <param name="id">addressbook id</param>
        /// <returns></returns>
        public AddressBook CheckConflict(string addressBookName, Guid? id)
        {
            var addressBook = _dbContext.AddressBook
                    .Include(addressBook => addressBook.Emails)
                    .Include(addressBook => addressBook.Phones)
                    .Include(addressBook => addressBook.Address)
                    .Include(addressBook => addressBook.Asset)
                    .FirstOrDefault(addressBook => addressBook.Id != id && addressBook.Name == addressBookName && addressBook.Active);
            return addressBook;
        }

        /// <summary>
        /// get the addressbook id by its addressbookname
        /// </summary>
        /// <returns></returns>
        public Guid getUserIdByUserName(string currentUser)
        {
            User addressbook = _dbContext.User.Include(addressbook => addressbook.UserSecret)
                        .FirstOrDefault(addressbook => (addressbook.UserSecret.UserName == currentUser) && addressbook.Active);
            return addressbook != null ? addressbook.Id : Guid.Empty;
        }

    }
}
