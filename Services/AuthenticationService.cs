using AddressBookApi.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using AddressBookApi.Dtos;
using System.Security.Cryptography;
using AutoMapper;
using Contracts;

namespace AddressBookApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository authenticationRepository;
        private readonly JwtSettings JwtSettings;
        private readonly IMapper mapper;

        public AuthenticationService(IAuthenticationRepository authenticationRepository, IOptions<JwtSettings> options, IMapper _mapper)
        {
            this.authenticationRepository = authenticationRepository;
            this.JwtSettings = options.Value;
            this.mapper = mapper;
        }

        private static string getHash(string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public async Task<TokenDto> AuthenticateUser(LoginDto loginInfo)
        {
            var user = await authenticationRepository.AuthenticateUser(loginInfo.UserName, getHash(loginInfo.Password));
            if (user is null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(this.JwtSettings.SecurityKey);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.UserName) }),
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDesc);
            string finalToken = tokenHandler.WriteToken(token);
            var tokenObject = new TokenDto()
            {
                AccessToken = finalToken,
                TokenType = "Bearer"
            };
            return tokenObject;
        }
    }
}
