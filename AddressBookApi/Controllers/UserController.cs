using Microsoft.AspNetCore.Mvc;
using Contracts;
using AddressBookApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AddressBookApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor for injecting logger and repository
        /// </summary>
        /// <param name="logger">logger object</param>
        /// <param name="userService">The service object</param>
        public UserController(ILoggerManager logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Get the current logged in user
        /// </summary>
        private string GetCurrentUser()
        {
            var identity = HttpContext?.User?.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            }
            return "";
        }

        /// <summary>
        /// Get users
        /// </summary>
        /// <remarks>Get the list of user.</remarks>
        /// <response code="200">Fetched the user list successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet("user")]
        [SwaggerOperation("GetUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<UserDto>), description: "Fetched the user list successfully")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult GetUser()
        {
            _logger.LogInfo("Get the total user details");
            return StatusCode(200, _userService.GetUser());
        }

        /// <summary>
        /// Get user by user id
        /// </summary>
        /// <remarks>Get the user by its id.</remarks>
        /// <param name="id"></param>
        /// <response code="200">Fetched the user successfully</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet("user/{id}")]
        [SwaggerOperation("GetUserById")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserDto), description: "Fetched the user successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponseDto), description: "Input Model is not valid")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponseDto), description: "User Id not found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult GetUserById(Guid id)
        {
            _logger.LogInfo($"Get user details of id {id}");
            return StatusCode(200, _userService.GetUserById(id));
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <remarks>User can be created by this api</remarks>
        /// <param name="body">Created user object</param>
        /// <response code="201">User Created Successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="409">Conflict occurred</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Authorize]
        [SwaggerOperation("CreateUser")]
        [SwaggerResponse(statusCode: 201, type: typeof(Guid), description: "User Created Successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponseDto), description: "Input Model is not valid")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 409, type: typeof(ErrorResponseDto), description: "Conflict occurred")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        [HttpPost("user")]
        public IActionResult CreateUser(CreateUserDto user)
        {
            _logger.LogInfo("Create a user");
            return StatusCode(201, _userService.CreateUser(user, GetCurrentUser()));
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <remarks>This api is used to update the user profile information.</remarks>
        /// <param name="id"></param>
        /// <param name="body">Update an existent user in the address book</param>
        /// <response code="200">User updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict occurred</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpPut("user/{id}")]
        [SwaggerOperation("UpdateUser")]
        [SwaggerResponse(statusCode: 204, description: "User updated successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponseDto), description: "Id is not valid")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 403, type: typeof(ErrorResponseDto), description: "Access is forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponseDto), description: "User Id not found")]
        [SwaggerResponse(statusCode: 409, type: typeof(ErrorResponseDto), description: "Conflict occurred")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult UpdateUser(Guid id, UserDto user)
        {
            _logger.LogInfo($"Update the user details of id {id}");
            return StatusCode(204, _userService.UpdateUser(id, user, GetCurrentUser()));
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <remarks>This api is used to delete the user record.</remarks>
        /// <param name="id"></param>
        /// <response code="204">User deleted successfully</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpDelete("user/{id}")]
        [SwaggerOperation("DeleteUser")]
        [SwaggerResponse(statusCode: 204, description: "User Deleted successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponseDto), description: "Id is not valid")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponseDto), description: "User Id not found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult DeleteUser(Guid id)
        {
            _logger.LogInfo($"Delete the user details of id {id}");
            return StatusCode(204, _userService.DeleteUser(id));
        }

        /// <summary>
        /// Get users count
        /// </summary>
        /// <remarks>Get the count of number of user.</remarks>
        /// <response code="200">Fetched the user count successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet("user/count")]
        [SwaggerOperation("GetUserCount")]
        [SwaggerResponse(statusCode: 200, type: typeof(CountDto), description: "Fetched the user count successfully")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult GetUserCount()
        {
            _logger.LogInfo("Get the total user count");
            return StatusCode(200, _userService.GetUserCount());
        }
    }
}
