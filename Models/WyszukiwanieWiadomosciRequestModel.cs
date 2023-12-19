namespace Quark_Backend.Models
{

        public class WyszukiwanieWiadomosciRequest
        {
            public string? FragmentWiadomosci { get; set; }
            public int? ConversationId { get; set; }
            public int? UserId { get; set; }
            public DateOnly? SentDate { get; set; }
        }
    
}
