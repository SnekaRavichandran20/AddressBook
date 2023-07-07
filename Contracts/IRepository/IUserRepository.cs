using AddressBookApi.Entities;
namespace Contracts;

public interface IUserRepository
{
    User CheckConflict(string userName, string email, Guid? id);
    Guid CreateUser(User user);
    bool DeleteUser(Guid id);
    List<User> GetUser();
    User GetUserById(Guid id);
    int GetUserCount();
    bool UpdateUser(Guid id, User user);
    Guid getUserIdByUserName(string? currentUser);
}

