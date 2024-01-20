namespace Quark_Backend.Models;


public class BasicConversationModel
{
    public List<Conversation> Conversations { get; set; }
    public class Conversation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; } //to let know what should be displayed on client side: name of conversation or name of user
        public List<User> Users { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PictureUrl { get; set; }
    }
}

