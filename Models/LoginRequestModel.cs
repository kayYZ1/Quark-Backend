namespace Quark_Backend.Models
{
    public class LoginRequestModel
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
