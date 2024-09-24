using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Teams.Domain.DTOs;
using Teams.Domain.Interfaces.Services;
using Teams.Domain.Models;
using Teams.Services.Services;

namespace Teams.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)    
        {
            var user = await _authService.AuthenticateUser(login);
            // Validate user credentials (in a real app, check against the database)
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = this.GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("G7u!9%kLq@T&d#2PzWx$8NvY^bF4A*J\r\n"));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //    claims: new[] { new Claim(JwtRegisteredClaimNames.Sub, "test") },
            //    expires: DateTime.Now.AddMinutes(30),
            //    signingCredentials: creds);

            //return new JwtSecurityTokenHandler().WriteToken(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("G7u!9%kLq@T&d#2PzWx$8NvY^bF4A*J\r\n");  // Use a secure key in production

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
                    // Add roles or other claims if needed
                }),
                Expires = DateTime.UtcNow.AddHours(1),  // Set expiration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    //public class LoginModel
    //{
    //    public string Username { get; set; }
    //    public string Password { get; set; }
    //}

}
