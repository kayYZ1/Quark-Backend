using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quark_Backend.DAL;
using Quark_Backend.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quark_Backend.Services
{

    public class TokenGeneration : ISecurityService
    {
        public string GenerateToken(string Email, string PermissionLevel, string Username) // Token generation for authorization purposes
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5iq-Very-Long-Secret-Key-For-Authorization-Purposes"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("PermissionLevel", PermissionLevel.ToString()),
                //new Claim(JwtRegisteredClaimNames.Name, Username) Throwing error while login in (first time login).
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
    }
}
