namespace Quark_Backend.Services
{
    public interface ISecurityService
    {
        Task<string> GenerateToken(string Email, string PermissionLevel, string Username);
    }
}