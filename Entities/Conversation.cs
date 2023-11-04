using System;
using System.Collections.Generic;

namespace Quark_Backend.Entities;

public partial class Conversation
{
    public int Id { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
