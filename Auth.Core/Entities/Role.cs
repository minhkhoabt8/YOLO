using System;
using System.Collections.Generic;

namespace Auth.Core.Entities;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();
}
