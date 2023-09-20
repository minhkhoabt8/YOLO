using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Entities
{
    public class RefreshToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.Now >= Expires;
        public int ExpiresIn => (int)Expires.Subtract(DateTime.Now).TotalSeconds;
        public Account Account { get; set; }
        public string AccountId { get; set; }
        public bool IsRevoked { get; set; } = false;
        public string? ReplacedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now.SetKindUtc();

        public void Revoke()
        {
            IsRevoked = true;
        }

        public void ReplaceWith(RefreshToken replacement)
        {
            ReplacedBy = replacement.Token;
            Revoke();
        }
    }
}
