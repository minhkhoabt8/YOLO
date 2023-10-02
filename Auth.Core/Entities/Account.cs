using System;
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

    public DateTime? OtpExpiredAt { get; set; }

    public bool IsActive { get; set; } = false;

    public bool IsDelete { get; set; } = false;

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();

    public virtual Role Role { get; set; } = null!;


    public void GernerateOTP()
    {
        Otp = new Random().Next(100000, 999999).ToString();
        OtpExpiredAt = DateTime.Now.AddMinutes(30);
    }

    public bool IsOtpValid(string checkOtp)
    {
        if (Otp != checkOtp || OtpExpiredAt < DateTime.Now )
            return false;
        return true;
        
    }
}
