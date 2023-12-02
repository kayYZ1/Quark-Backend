namespace Quark_Backend.Models
{
  public class UserInfoModel
  {
    public required string Email { get; set; }
    public string? Username { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? SelfDescription { get; set; }
    public string? PictureUrl { get; set; }
  }
}