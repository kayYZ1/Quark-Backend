namespace Quark_Backend.Models
{
    public class ConversationMessagesModel
    {
        public class Message
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Text { get; set; }
            public DateOnly Date { get; set; }
            public string Timestamp { get; set; }
        }

        public List<Message> Messages = new List<Message>();
    }
}
