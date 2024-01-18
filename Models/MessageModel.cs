namespace Quark_Backend.Models
{
    public class MessageModel
    {
        public string Content { get; set; }
        public string Username { get; set; }
        public string Timestamp { get; set; }
        public string Text { get; set; } = null!;
    }
}
