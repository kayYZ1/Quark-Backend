using System;
using System.Collections.Generic;

namespace Quark_Backend.Entities;

public partial class Announcement
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Content { get; set; }
    public required string Time {get; set;}
    public int UserId {get; set;}

    public virtual User User { get; set; } = null!;
}