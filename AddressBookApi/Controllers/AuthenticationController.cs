using Microsoft.AspNetCore.Mvc;
using AddressBookApi.Dtos;
using Contracts;

namespace AddressBookApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> AuthenticateUserAsync(LoginDto loginInfo)
        {
            var token = await authenticationService.AuthenticateUser(loginInfo);
            if (token is null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}

