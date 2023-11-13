using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quark_Backend.DAL;
using Quark_Backend.Entities;
using Quark_Backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quark_Backend.Controllers
{
    [Authorize(Policy = "PermissionLevel1")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly QuarkDbContext _dbContext;
        
        private readonly LoginRequestModel _loginRequest; // Model containing Email and Password
        public LoginController(QuarkDbContext context, LoginRequestModel loginRequest)
        {
            _dbContext = context;
            _loginRequest = loginRequest;
        }
        
        public string GenerateToken(User user) // Token generation for authorization purposes
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5iq-Very-Long-Secret-Key-For-Authorization-Purposes"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("PermissionLevel", user.PermissionLevel.ToString()),
    };

            var token = new JwtSecurityToken(
                "QuarkApp",
                "User",
                claims,
                expires: DateTime.UtcNow.AddHours(8), // Set token expiration time
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {

            // Find the user by email in the database
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var hashedPasswordFromDatabase = user.Password; // Retrieve the hashed password from the database
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(model.Password, hashedPasswordFromDatabase);

 
            if (!passwordMatch)
            {
                return Unauthorized("Invalid email or password");
            }

            var token = GenerateToken(user);

            return Ok(new { Token = token });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
