using AddressBookApi.Entities;
using AddressBookApi.Models.Data;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AddressBookDataContext _dbContext;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor for injecting logger and db context
        /// </summary>
        /// <param name="logger">logger object</param>
        /// <param name="dbContext">The db context object</param>
        public UserRepository(AddressBookDataContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// creates user in the database
        /// </summary>
        /// <param name="user">the user model that has to be created</param>
        /// <returns></returns>
        public Guid CreateUser(User user)
        {
            _dbContext.User.AddAsync(user);
            _dbContext.SaveChangesAsync();
            return user.Id;
        }

        /// <summary>
        /// fetch user from the database
        /// </summary>
        /// <returns></returns>
        public List<User> GetUser()
        {
            List<User> user = _dbContext.User.Include(user => user.UserSecret).Where(user => user.Active).ToList();
            return user;
        }

        /// <summary>
        /// fetch user by its id from the database
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        public User GetUserById(Guid id)
        {
            User user = _dbContext.User.Include(user => user.UserSecret).FirstOrDefault(user => user.Id == id && user.Active);
            return user;
        }

        /// <summary>
        /// update user in the database
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="user">the user model that has to be updated</param>
        /// <returns></returns>
        public bool UpdateUser(Guid id, User user)
        {
            User oldDataOfUser = GetUserById(id);
            if (oldDataOfUser is null)
            {
                return false;
            }
            _dbContext.Entry(oldDataOfUser).CurrentValues.SetValues(user);
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// deletes user in the database
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        public bool DeleteUser(Guid id)
        {
            User userToDelete = GetUserById(id);
            if (userToDelete != null)
            {
                userToDelete.Active = false;
                userToDelete.UserSecret.Active = false;
                _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// get the user count
        /// </summary>
        /// <returns></returns>
        public int GetUserCount()
        {
            return _dbContext.User.Include(user => user.UserSecret).Where(user => user.Active).Count();
        }

        /// <summary>
        /// check for conflict of username or email
        /// </summary>
        /// <param name="userName">username that needs to check for conflict</param>
        /// <param name="email">email that needs to check for conflict</param>
        /// <param name="id">user id</param>
        /// <returns></returns>
        public User CheckConflict(string userName, string email, Guid? id)
        {
            User user = _dbContext.User.Include(user => user.UserSecret)
                        .FirstOrDefault(user => user.Id != id &&
                            (user.UserSecret.UserName == userName || user.EmailAddress == email) && user.Active);
            return user;

        }

        /// <summary>
        /// get the user id by its username
        /// </summary>
        /// <returns></returns>
        public Guid getUserIdByUserName(string currentUser)
        {
            User user = _dbContext.User.Include(user => user.UserSecret)
                        .FirstOrDefault(user => (user.UserSecret.UserName == currentUser) && user.Active);
            return user != null ? user.Id : Guid.Empty;
        }
    }
}
