using Microsoft.AspNetCore.Identity;

namespace StudyBuddy.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // FK
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Revoked { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        public IdentityUser User { get; set; }  // Navigation property
    }
}
