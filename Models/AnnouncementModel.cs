namespace Quark_Backend.Models
{
    public class AnnouncementModel
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Time {get; set;}
        public required string Email {get; set;}
    }
}
