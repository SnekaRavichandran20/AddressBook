using AddressBookApi.Dtos;
using AddressBookApi.Entities;
namespace Contracts;

public interface IAddressBookService
{
    AddressBook CheckConflict(string addressBookName, Guid? id);
    Guid CreateAddressBook(CreateAddressBookDto addressBook, string currentUser);
    bool DeleteAddressBook(Guid id);
    List<AddressBookDto> GetAddressBook();
    AddressBookDto GetAddressBookById(Guid id);
    CountDto GetAddressBookCount();
    bool UpdateAddressBook(Guid id, EditAddressBookDto addressBook, string currentUser);
}