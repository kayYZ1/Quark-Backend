namespace Quark_Backend.Models
{
    public class UserEdit
    { 
        public string? Password { get; set; }
        public int? PermissionLevel { get; set; }

        public string? Email {  get; set; }
        public int? JobId { get; set; }


    }
}
