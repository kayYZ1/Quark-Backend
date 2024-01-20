namespace Quark_Backend.Services
{
    public interface ISecurityService
    {
        string GenerateToken(string Email, string PermissionLevel, string Username);
    }
}