using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;

namespace Auth.Core.Entities;

public partial class Notification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string UserId { get; set; } = null!;

    public string NotificationContent { get; set; } = null!;

    public DateTime CreatedDate { get; set; } = DateTime.Now.SetKindUtc();

    public bool IsRead { get; set; } = false;

    public bool IsDeleted { get; set; } = false;

    public virtual Account User { get; set; } = null!;


    public void GenerateNotification(string userId, string content)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        NotificationContent = content;
    }
}
