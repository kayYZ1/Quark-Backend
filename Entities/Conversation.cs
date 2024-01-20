using System;
using System.Collections.Generic;

namespace Quark_Backend.Entities;

public partial class Conversation
{
    public int Id { get; set; }
    public string Name { get; set; }//equal to group name
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
