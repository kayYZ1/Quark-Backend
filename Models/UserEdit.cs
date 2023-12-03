namespace Quark_Backend.Models
{
    public class UserEdit
    {
        public int Id { get; set; }
        public string? Password { get; set; }
        public int? PermissionLevel { get; set; }

        public string? Email {  get; set; }
        public int? JobId { get; set; }


    }
}
