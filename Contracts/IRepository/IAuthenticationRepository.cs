using AddressBookApi.Entities;
namespace Contracts;

public interface IAuthenticationRepository
{
    Task<UserSecret> AuthenticateUser(string userName, string password);
}



