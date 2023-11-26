using System.ComponentModel.DataAnnotations;

namespace Quark_Backend.Models
{
  public class UserInfoModel
  {
    [Required]
    public string Email { get; set; }
    public string? Username { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string? SelfDescription { get; set; }
    public string? PictureUrl { get; set; }
  }
}