using System.ComponentModel.DataAnnotations;
using ThucTapProject.Entities;

namespace ThucTapProject.EntityModel
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public string Token { get; set; }
        public string JwtId { get; set; } // Access Token ID
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IsUsedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
