using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Contracts;
using AddressBookApi.Dtos;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace AddressBookApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookService _addressBookService;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor for injecting logger and repository
        /// </summary>
        /// <param name="logger">logger object</param>
        /// <param name="addressBookService">The service object</param>
        public AddressBookController(IAddressBookService addressBookService, ILoggerManager logger)
        {
            _addressBookService = addressBookService;
            _logger = logger;
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
        /// Get address books
        /// </summary>
        /// <remarks>Get the list of address for the user.</remarks>
        /// <response code="200">Fetched the list of addressbook successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet("address-book")]
        [SwaggerOperation("GetAddressBooks")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<AddressBookDto>), description: "Fetched the list of addressbook successfully")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult GetAddressBook()
        {
            _logger.LogInfo("Get the total addressbook details");
            return StatusCode(200, _addressBookService.GetAddressBook());
        }

        /// <summary>
        /// Get addressbook by id
        /// </summary>
        /// <remarks>Getting the addressbook by id.</remarks>
        /// <param name="id"></param>
        /// <response code="200">Fetched the addressbook successfully</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet("address-book/{id}")]
        [SwaggerOperation("GetAddressBookById")]
        [SwaggerResponse(statusCode: 200, type: typeof(AddressBookDto), description: "Fetched the addressbook successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponseDto), description: "Input Model is not valid")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponseDto), description: "AddressBook Id not found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult GetAddressBookById(Guid id)
        {
            _logger.LogInfo($"Get addressbook details of id {id}");
            return StatusCode(200, _addressBookService.GetAddressBookById(id));
        }

        /// <summary>
        /// Create address
        /// </summary>
        /// <remarks>Create address for the user.</remarks>
        /// <param name="body">Created AddressBook object</param>
        /// <response code="201">AddressBook Created Successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="409">Conflict occurred</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpPost("address-book")]
        [SwaggerOperation("CreateAddressBook")]
        [SwaggerResponse(statusCode: 201, type: typeof(string), description: "AddressBook Created Successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponseDto), description: "Input Model is not valid")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 409, type: typeof(ErrorResponseDto), description: "Conflict Occurred")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult CreateAddressBook(CreateAddressBookDto addressBook)
        {
            _logger.LogInfo("Create a addressbook");
            return StatusCode(201, _addressBookService.CreateAddressBook(addressBook, GetCurrentUser()));
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <remarks>This api is used to update the addressbook details</remarks>
        /// <param name="id"></param>
        /// <param name="body">Update an existent address book</param>
        /// <response code="200">User updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User not found</response>
        /// <response code="409">Conflict occurred</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpPut("address-book/{id}")]
        [SwaggerOperation("UpdateAddressBook")]
        [SwaggerResponse(statusCode: 204, description: "AddressBook updated successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponseDto), description: "Id is not valid")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponseDto), description: "AddressBook Id not found")]
        [SwaggerResponse(statusCode: 409, type: typeof(ErrorResponseDto), description: "Conflict Occurred")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult UpdateAddressBook(Guid id, EditAddressBookDto addressBook)
        {
            _logger.LogInfo($"Update the addressbook details of id {id}");
            return StatusCode(204, _addressBookService.UpdateAddressBook(id, addressBook, GetCurrentUser()));
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <remarks>This api is used to delete the addressbook record</remarks>
        /// <param name="id"></param>
        /// <response code="204">User deleted successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpDelete("address-book/{id}")]
        [SwaggerOperation("DeleteAddressBook")]
        [SwaggerResponse(statusCode: 204, description: "AddressBook Deleted successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponseDto), description: "Id is not valid")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponseDto), description: "AddressBook Id not found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult DeleteAddressBook(Guid id)
        {
            _logger.LogInfo($"Delete the addressbook details of id {id}");
            return StatusCode(204, _addressBookService.DeleteAddressBook(id));
        }

        /// <summary>
        /// Get addressbook count
        /// </summary>
        /// <remarks>Get the count of number of contact details in total.</remarks>
        /// <response code="200">Fetched the addressbook count successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet("address-book/count")]
        [SwaggerOperation("GetAddressBookCount")]
        [SwaggerResponse(statusCode: 200, type: typeof(CountDto), description: "Fetched the addressbook count successfully")]
        [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponseDto), description: "User is not authorized")]
        [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponseDto), description: "Internal server error")]
        public IActionResult GetAddressBookCount()
        {
            _logger.LogInfo("Get the total addressbook count");
            return StatusCode(200, _addressBookService.GetAddressBookCount());
        }
    }
}
