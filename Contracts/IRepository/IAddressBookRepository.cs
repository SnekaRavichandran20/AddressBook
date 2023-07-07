using AddressBookApi.Entities;
namespace Contracts;

public interface IAddressBookRepository
{
    AddressBook CheckConflict(string addressBookName, Guid? id);
    Guid CreateAddressBook(AddressBook addressBook);
    bool DeleteAddressBook(Guid id);
    List<AddressBook> GetAddressBook();
    AddressBook GetAddressBookById(Guid id);
    int GetAddressBookCount();
    bool UpdateAddressBook(Guid id, AddressBook addressBook);
    Guid getUserIdByUserName(string? currentUser);
}

