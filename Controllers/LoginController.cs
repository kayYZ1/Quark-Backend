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
    //[Authorize(Policy = "PermissionLevel1")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly QuarkDbContext _dbContext;
        
      
        public LoginController(QuarkDbContext context)
        {
            _dbContext = context;
        }
        [HttpGet]
        public string GenerateToken(string Email,string PermissionLevel) // Token generation for authorization purposes
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5iq-Very-Long-Secret-Key-For-Authorization-Purposes"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("PermissionLevel", PermissionLevel.ToString()),
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



        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {

            // Find the user by email in the database
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }
            // Version for open password 
            if (model.Password == user.Password)
            {
                return Ok(GenerateToken(user.Email, user.PermissionLevel.ToString()));
            }
            else
            {
                return Unauthorized("Invalid email or password");
            }
            // Version for hashed password


            //var hashedPasswordFromDatabase = user.Password; // Retrieve the hashed password from the database
            //bool passwordMatch = BCrypt.Net.BCrypt.Verify(model.Password, hashedPasswordFromDatabase);
            //if (!passwordMatch)
            //{
            //   return Unauthorized("Invalid email or password");
            //}
            //return Ok(GenerateToken(user.Email, user.PermissionLevel.ToString()));



        }
    }
}
