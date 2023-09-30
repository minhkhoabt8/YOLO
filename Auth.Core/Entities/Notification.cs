using System;
using System.Collections.Generic;

namespace Auth.Core.Entities;

public partial class Notification
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string NotificationContent { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public bool IsRead { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Account User { get; set; } = null!;
}
