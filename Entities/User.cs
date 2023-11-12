using System;
using System.Collections.Generic;

namespace Quark_Backend.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;
    public string? Username {get; set; }

    public string? Password { get; set; }

    public string? SelfDescription { get; set; }
    public string? PictureUrl {get; set;}

    public int? PermissionLevel { get; set; }

    public int? JobId { get; set; }

    public virtual JobPosition? JobPosition { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
