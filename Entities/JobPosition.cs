using System;
using System.Collections.Generic;

namespace Quark_Backend.Entities;

public partial class JobPosition
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
