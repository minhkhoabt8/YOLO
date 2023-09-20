﻿using System;
using System.Collections.Generic;

namespace Auth.Core.Entities;

public partial class Account
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Otp { get; set; }

    public bool? IsActive { get; set; } = false;

    public bool? IsDelete { get; set; } = false;

    public virtual Role Role { get; set; } = null!;
    public virtual ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();
}

