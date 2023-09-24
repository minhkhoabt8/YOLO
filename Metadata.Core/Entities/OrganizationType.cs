using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class OrganizationType
{
    public string OrganizationTypeId { get; set; } = null!;

    public string? Code { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Owner> Owners { get; } = new List<Owner>();
}
