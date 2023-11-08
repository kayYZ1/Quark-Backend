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
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly QuarkDbContext _dbContext;
        // Model containing Email and Password
        private readonly LoginRequestModel _loginRequest;
        public LoginController(QuarkDbContext context)
        {
            _dbContext = context;
            _loginRequest = new LoginRequestModel(); 
        }
        // Token generation for authentication
        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyTest"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

            var token = new JwtSecurityToken(
                "IssuerTest",
                "AudienceTest",
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

            // Check if the provided password matches the stored password (without encryption)
            if (!passwordMatch)
            {
                return Unauthorized("Invalid email or password");
            }

            // Token library (e.g., JWT) to generate an authentication token
            var token = GenerateToken(user);

            return Ok(new { Token = token });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
