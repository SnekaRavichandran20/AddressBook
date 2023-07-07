using AddressBookApi.Entities;
using System.Text;
using AddressBookApi.Dtos;
using System.Security.Cryptography;
using AutoMapper;
using Contracts;
using ExceptionHandler;
using System.Net;
using System.Net.Mail;

namespace AddressBookApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor for injecting logger, mapper and repository
        /// </summary>
        /// <param name="logger">logger object</param>
        /// <param name="mapper">mapper object</param>
        /// <param name="userRepository">The repository object</param>
        public UserService(ILoggerManager logger, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Method for hashing the password
        /// </summary>
        private static string getHash(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Method to check whether the email id is valid or not
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                throw new ConflictCustomException("Bad Request", "Invalid email id");
            }
        }

        /// <summary>
        /// Method to create a user
        /// </summary>
        /// <param name="user">The user details to be created</param>
        /// <param name="currentUser">The current logged in user</param>
        /// <returns name= "id">user id</returns>
        public Guid CreateUser(CreateUserDto user, string currentUser)
        {
            var checkConflict = CheckConflict(user.UserName, user.EmailAddress, null);
            if (checkConflict != null)
            {
                _logger.LogInfo("Conflict occurred");
                throw new ConflictCustomException("Conflict Occurs", "UserName/EmailAddress already exist");
            }
            IsValidEmail(user.EmailAddress);
            Guid userId = _userRepository.getUserIdByUserName(currentUser);
            User userToCreate = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                CreatedBy = userId,
                ModifiedBy = Guid.Empty,
                Active = true,
                Role = user.Role,
                UserSecret = new UserSecret()
                {
                    Id = Guid.NewGuid(),
                    UserName = user.UserName,
                    Password = getHash(user.Password),
                    Active = true
                }
            };
            Guid id = _userRepository.CreateUser(userToCreate);
            if (id == null)
            {
                throw new InternalServerCustomException("Internal Server Error", "Internal Server Error");
            }
            _logger.LogInfo("User created successfully");
            return id;
        }


        /// <summary>
        /// Method to fetch the total user
        /// </summary>
        /// <returns name= "List<UserDto>">List of all users</returns>
        public List<UserDto> GetUser()
        {
            List<User> users = _userRepository.GetUser();
            List<UserDto> usersByDto = new List<UserDto>();

            foreach (User user in users)
            {
                UserDto entity = new UserDto()
                {
                    Id = user.Id,
                    UserName = user.UserSecret.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.EmailAddress,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role
                };
                usersByDto.Add(entity);
            }
            _logger.LogInfo("User details fetched successfully");
            return usersByDto;
        }

        /// <summary>
        /// Method to get the user detail by providing the user id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns name="userByDto">User detail of the provided id</returns>
        public UserDto GetUserById(Guid id)
        {
            User user = _userRepository.GetUserById(id);

            if (user is null)
            {
                _logger.LogInfo($"The user id {id} was not found");
                throw new NotFoundCustomException("Id not found", "Id not found"); ;
            }
            UserDto userByDto = new UserDto()
            {
                Id = user.Id,
                UserName = user.UserSecret.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };
            _logger.LogInfo("User detail fetched successfully");
            return userByDto;
        }

        /// <summary>
        /// Method to update the user detail by providing the user id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <param name="user">User detail of the id</param>
        /// <param name="currentUser">the current user who logged in</param>
        /// <returns name="response">response whether the user is updated or not</returns>
        public bool UpdateUser(Guid id, UserDto user, string currentUser)
        {
            if (id != user.Id)
            {
                _logger.LogInfo($"Mismatch occurred in the {id}");
                throw new BadRequestCustomException("Bad Request", "Bad Request");
            }
            User userToUpdate = _userRepository.GetUserById(id);
            if (userToUpdate is null)
            {
                _logger.LogInfo($"The user id {id} was not found");
                throw new NotFoundCustomException("User not found", "User not found");
            }
            if (userToUpdate.Role.ToLower() != "admin" && (userToUpdate.EmailAddress != user.Email || userToUpdate.Role != user.Role))
            {
                _logger.LogInfo($"The user not having permission to update the role or email address");
                throw new ForbiddenCustomException("Forbidden", "Forbidden");
            }
            var checkConflict = CheckConflict(user.UserName, user.Email, null);
            if (checkConflict != null)
            {
                _logger.LogInfo("Conflict Occurred");
                throw new ConflictCustomException("Conflict Occurs", "UserName/EmailAddress already exist");
            }
            IsValidEmail(user.Email);
            Guid userId = _userRepository.getUserIdByUserName(currentUser);
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.EmailAddress = user.Email;
            userToUpdate.PhoneNumber = user.PhoneNumber;
            userToUpdate.ModifiedOn = DateTime.UtcNow;
            userToUpdate.ModifiedBy = userId;
            userToUpdate.Role = user.Role;
            userToUpdate.UserSecret.UserName = user.UserName;

            bool response = _userRepository.UpdateUser(user.Id, userToUpdate);
            _logger.LogInfo($"The user of {id} updated successfully");

            return response;
        }


        /// <summary>
        /// Method to delete the user by providing the user id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns name="response">response whether the user is deleted or not</returns>
        public bool DeleteUser(Guid id)
        {
            User userToUpdate = _userRepository.GetUserById(id);
            if (userToUpdate is null)
            {
                _logger.LogInfo($"The user id {id} was not found");
                throw new NotFoundCustomException("User not found", "User not found");
            }
            bool response = _userRepository.DeleteUser(id);
            _logger.LogInfo($"The user of {id} deleted successfully");
            return response;
        }

        /// <summary>
        /// Method to get the number of users
        /// </summary>
        /// <returns name="count">total number of users</returns>
        public CountDto GetUserCount()
        {
            int count = _userRepository.GetUserCount();
            CountDto countByDto = new CountDto()
            {
                Count = count
            };
            _logger.LogInfo($"The total number of users were {count}");
            return countByDto;
        }

        /// <summary>
        /// Method to find conflict in username or email
        /// </summary>
        public User CheckConflict(string userName, string email, Guid? id)
        {
            return _userRepository.CheckConflict(userName, email, id);
        }
    }
}