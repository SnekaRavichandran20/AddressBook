using AddressBookApi.Entities;
using AddressBookApi.Models.Data;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AddressBookApi.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly AddressBookDataContext dbContext;
        private readonly ILogger logger;
        
        public AuthenticationRepository(AddressBookDataContext _dbContext, ILogger<AuthenticationRepository> _logger)
        {
            dbContext = _dbContext;
            logger = _logger;
        }

        public async Task<UserSecret> AuthenticateUser(string userName, string password)
        {
            var user = await this.dbContext.UserSecret.FirstOrDefaultAsync(item => item.UserName == userName && item.Password == password && item.Active);
            return user;
        }
    }
}
