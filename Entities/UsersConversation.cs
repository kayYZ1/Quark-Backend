using System;
using System.Collections.Generic;

namespace Quark_Backend.Entities;

public partial class UsersConversation
{
    public int UsersId { get; set; }

    public int ConversationsId { get; set; }

    public virtual Conversation Conversations { get; set; } = null!;

    public virtual User Users { get; set; } = null!;
}
