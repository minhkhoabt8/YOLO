using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;

namespace Auth.Core.Entities;

public partial class Notification
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string NotificationContent { get; set; } = null!;

    public DateTime CreatedDate { get; set; } = DateTime.Now.SetKindUtc();

    public bool IsRead { get; set; } = false;

    public bool IsDeleted { get; set; } = false;

    public virtual Account User { get; set; } = null!;


    public void GenerateNotification(string userUd, string content)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userUd;
        NotificationContent = content;
    }
}
