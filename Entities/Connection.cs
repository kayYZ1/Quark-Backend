using System;
using System.Collections.Generic;

namespace Quark_Backend.Entities;

public partial class Connection
{
    public int Id { get; set; }
    public string UserAgent { get; set; }
    public bool State { get; set; } // is connection active
    public int UserId { get; set; }
    public virtual User User { get; set; }
}
