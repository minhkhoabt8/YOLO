using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class OrganizationType
{
    public string OrganizationTypeId { get; set; } = Guid.NewGuid().ToString();

    public string? Code { get; set; }

    public string? Name { get; set; }
    public bool? IsDeleted { get; set; } = false;

    public virtual ICollection<Owner> Owners { get; } = new List<Owner>();
}
