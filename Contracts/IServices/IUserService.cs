using AddressBookApi.Dtos;
using AddressBookApi.Entities;
namespace Contracts;

public interface IUserService
{
    User CheckConflict(string userName, string email, Guid? id);
    Guid CreateUser(CreateUserDto user, string currentUser);
    bool DeleteUser(Guid id);
    List<UserDto> GetUser();
    UserDto GetUserById(Guid id);
    CountDto GetUserCount();
    bool UpdateUser(Guid id, UserDto user, string currentUser);
}