using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class LogError
{
    [Key]
    public string ErrorId { get; set; } = Guid.NewGuid().ToString();

    public int? StatusCode { get; set; }

    public string? ErrorInfo { get; set; }

    public string? Type { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow.AddHours(7);

    public bool? IsDeleted { get; set; } = false;
}
