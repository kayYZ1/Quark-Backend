using System;
using System.Collections.Generic;

namespace Quark_Backend.Entities;

public partial class Message
{
    public int Id { get; set; }

    public DateOnly SentDate { get; set; }

    public string Text { get; set; } = null!;

    public int UserId { get; set; }

    public int ConversationId { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
