using AddressBookApi.Dtos;
namespace Contracts;
public interface IAuthenticationService
{
    Task<TokenDto> AuthenticateUser(LoginDto login);
}